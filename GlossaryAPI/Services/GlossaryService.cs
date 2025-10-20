using GlossaryAPI.DTOs;
using GlossaryAPI.Interfaces;
using GlossaryAPI.Models;


namespace GlossaryAPI.Services
{
    public class GlossaryService : IGlossaryService
    {
        private readonly IGlossaryRepository _repositoryGlossary;
        private readonly GlossaryTermValidator _validator;
        private readonly IUserRepository _repositoryUsers;
        public GlossaryService(IGlossaryRepository repository, IUserRepository repositoryUsers, GlossaryTermValidator validator)
        {
            _repositoryGlossary = repository;
            _repositoryUsers = repositoryUsers;
            _validator = validator;
            
        }

        public IEnumerable<GlossaryTermDTO> GetAllTerms()
        {
            var items = _repositoryGlossary.GetAll()
            .Select(item => new GlossaryTermDTO
            {
                Id = item.Id,
                Term = item.Term,
                Definition = item.Definition,
                Status = item.Status
            })
            .ToList();

            return items;
        }

        public GlossaryTermDTO? GetTermById(int id)
        {
            var term = _repositoryGlossary.GetById(id);
            if (term == null)
                return null;
            
            var termDto = new GlossaryTermDTO
            {
                Id = term.Id,
                Term = term.Term,
                Definition = term.Definition,
                Status = term.Status
            };

            return termDto;
        }

        public GlossaryTermDTO CreateTerm(GlossaryTermDTO newTermDto, int userId)
        {
            if (newTermDto == null)
                throw new ArgumentNullException(nameof(newTermDto));

            User currentUser = GetUserFromDb(userId);

            var newTerm = new GlossaryTerm
            {
                Term = newTermDto.Term,
                Definition = newTermDto.Definition,
                CreatedBy = currentUser.Username,
                Status = ItemStatus.Draft
            };

            _repositoryGlossary.Add(newTerm);
            _repositoryGlossary.SaveChanges();
            
            newTermDto.Id = newTerm.Id;
            newTermDto.Status = newTerm.Status;

            return newTermDto;
        }

        public GlossaryTermDTO UpdateTerm(GlossaryTermDTO updatedTerm, int userId)
        {
            var existingTerm = _repositoryGlossary.GetById(updatedTerm.Id);
            if (existingTerm == null)
                throw new KeyNotFoundException($"Term with ID {updatedTerm.Id} not found");

            User currentUser = GetUserFromDb(userId);

            existingTerm.Term = updatedTerm.Term;
            existingTerm.Definition = updatedTerm.Definition;
            existingTerm.CreatedBy = currentUser.Username;
            existingTerm.Status = updatedTerm.Status;

            _repositoryGlossary.SaveChanges();
            
            var termDto = new GlossaryTermDTO
            {
                Id = existingTerm.Id,
                Term = existingTerm.Term,
                Definition = existingTerm.Definition,
                Status = existingTerm.Status
            };

            return termDto;
        }

      
        public GlossaryTermDTO PublishTerm(GlossaryTermDTO updatedTerm, int userId)
        {
            var existingTerm = _repositoryGlossary.GetById(updatedTerm.Id); ;
            if (existingTerm == null)
                throw new KeyNotFoundException($"Term with ID {updatedTerm.Id} not found");

            _validator.ValidateTermForPublish(updatedTerm);

            User currentUser = GetUserFromDb(userId);

            existingTerm.Term = updatedTerm.Term;
            existingTerm.Definition = updatedTerm.Definition;
            existingTerm.CreatedBy = currentUser.Username;
            existingTerm.Status = ItemStatus.Published;

            _repositoryGlossary.SaveChanges();

            var termDto = new GlossaryTermDTO
            {
                Id = existingTerm.Id,
                Term = existingTerm.Term,
                Definition = existingTerm.Definition,
                Status = existingTerm.Status
            };

            return termDto;
        }
        public GlossaryTermDTO ArchiveTerm(int id, int userId)
        {
            var existingTerm = _repositoryGlossary.GetById(id);
            if (existingTerm == null)
                throw new KeyNotFoundException($"Term with ID {id} not found");

            if (existingTerm.Status != ItemStatus.Published)
                throw new InvalidOperationException("Only published terms can be archived");

            User currentUser = GetUserFromDb(userId);
            if (existingTerm.CreatedBy != currentUser.Username)
                throw new UnauthorizedAccessException("You can only archive terms created by you");

            existingTerm.Status = ItemStatus.Archived;
            _repositoryGlossary.SaveChanges();

            var termDto = new GlossaryTermDTO
            {
                Id = existingTerm.Id,
                Term = existingTerm.Term,
                Definition = existingTerm.Definition,
                Status = existingTerm.Status
            };

            return termDto;
        }

        public void DeleteTerm(int id, int userId)
        {

            var term = _repositoryGlossary.GetById(id);
            if (term == null)
                throw new KeyNotFoundException($"Term with ID {id} not found");

            if (term.Status != ItemStatus.Draft)
                throw new InvalidOperationException("Only draft terms can be deleted");
            
            User currentUser = GetUserFromDb(userId);
            if (term.CreatedBy != currentUser.Username)
                throw new UnauthorizedAccessException("You can only delete terms that you have created"); //da li je ovo dobar tip greske

            _repositoryGlossary.Delete(term);
            _repositoryGlossary.SaveChanges();
        }

        private User GetUserFromDb(int userId)
        {
            var user = _repositoryUsers.GetById(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with userId {userId} not found");
            return user;
        }
    }

}
