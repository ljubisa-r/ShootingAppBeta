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
            .Where(item => item.Status != ItemStatus.Archived)
            .Select(item => new GlossaryTermDTO
            {
                id = item.Id,
                term = item.Term,
                definition = item.Definition,
                status = item.Status,
                createdBy = item.CreatedBy
            })
            .ToList();

            return items;
        }

        public GlossaryTermDTO? GetTermById(int id)
        {
            var term = _repositoryGlossary.GetById(id);
            if (term == null)
                return null;

            return MapToDto(term);
        }

        public GlossaryTermDTO CreateTerm(GlossaryTermDTO newTermDto, int userId)
        {
            if (newTermDto == null)
                throw new ArgumentNullException(nameof(newTermDto));

            User currentUser = GetUserFromDb(userId);

            var newTerm = new GlossaryTerm
            {
                Term = newTermDto.term,
                Definition = newTermDto.definition,
                CreatedBy = currentUser.Id,
                Status = ItemStatus.Draft
            };

            _repositoryGlossary.Add(newTerm);
            _repositoryGlossary.SaveChanges();

            return MapToDto(newTerm);
        }

        public GlossaryTermDTO UpdateTerm(GlossaryTermDTO updatedTerm, int userId)
        {
            var existingTerm = _repositoryGlossary.GetById(updatedTerm.id);
            if (existingTerm == null)
                throw new KeyNotFoundException($"Term with ID {updatedTerm.id} not found");

            User currentUser = GetUserFromDb(userId);

            existingTerm.Term = updatedTerm.term;
            existingTerm.Definition = updatedTerm.definition;
            existingTerm.CreatedBy = currentUser.Id;
            existingTerm.Status = updatedTerm.status;

            _repositoryGlossary.SaveChanges();

            return MapToDto(existingTerm);
        }

      
        public GlossaryTermDTO PublishTerm(GlossaryTermDTO updatedTerm, int userId)
        {
            var existingTerm = _repositoryGlossary.GetById(updatedTerm.id);
            if (existingTerm == null)
                throw new KeyNotFoundException($"Term with ID {updatedTerm.id} not found");

            _validator.ValidateTermForPublish(updatedTerm);

            User currentUser = GetUserFromDb(userId);

            existingTerm.Term = updatedTerm.term;
            existingTerm.Definition = updatedTerm.definition;
            existingTerm.CreatedBy = currentUser.Id;
            existingTerm.Status = ItemStatus.Published;

            _repositoryGlossary.SaveChanges();

            return MapToDto(existingTerm);
        }
        public GlossaryTermDTO ArchiveTerm(int id, int userId)
        {
            var existingTerm = _repositoryGlossary.GetById(id);
            if (existingTerm == null)
                throw new KeyNotFoundException($"Term with ID {id} not found");

            if (existingTerm.Status != ItemStatus.Published)
                throw new InvalidOperationException("Only published terms can be archived");

            existingTerm.Status = ItemStatus.Archived;
            _repositoryGlossary.SaveChanges();

            return MapToDto(existingTerm);
        }

        public void DeleteTerm(int id, int userId)
        {

            var term = _repositoryGlossary.GetById(id);
            if (term == null)
                throw new KeyNotFoundException($"Term with ID {id} not found");

            if (term.Status != ItemStatus.Draft)
                throw new InvalidOperationException("Only draft terms can be deleted");
            
            User currentUser = GetUserFromDb(userId);
            if (term.CreatedBy != currentUser.Id)
                throw new InvalidOperationException("You can only delete terms that you have created");

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

        private GlossaryTermDTO MapToDto(GlossaryTerm existingTerm)
        {
            if (existingTerm == null)
                throw new ArgumentNullException(nameof(existingTerm));

            return new GlossaryTermDTO
            {
                id = existingTerm.Id,
                term = existingTerm.Term,
                definition = existingTerm.Definition,
                status = existingTerm.Status
            };
        }
    }

}
