using GlossaryAPI.DTOs;
using GlossaryAPI.Interfaces;
using GlossaryAPI.Models;


namespace GlossaryAPI.Services
{
    public class GlossaryService : IGlossaryService
    {
        private readonly IGlossaryRepository _repositoryGlossary;
        private readonly GlossaryTermValidator _validator;
        public GlossaryService(IGlossaryRepository repository, GlossaryTermValidator validator)
        {
            _repositoryGlossary = repository;
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

        public GlossaryTermDTO CreateTerm(GlossaryTermDTO newTermDto)
        {
            if (newTermDto == null)
                throw new ArgumentNullException(nameof(newTermDto));

            var newTerm = new GlossaryTerm
            {
                Term = newTermDto.Term,
                Definition = newTermDto.Definition,
                CreatedBy = newTermDto.CreatedBy ?? "Unknown", // Add user from context in real app
                Status = ItemStatus.Draft
            };

            _repositoryGlossary.Add(newTerm);
            _repositoryGlossary.SaveChanges();
            
            newTermDto.Id = newTerm.Id;
            newTermDto.Status = newTerm.Status;

            return newTermDto;
        }

        public GlossaryTermDTO UpdateTerm(GlossaryTermDTO updatedTerm)
        {
            var existingTerm = _repositoryGlossary.GetById(updatedTerm.Id);
            if (existingTerm == null)
                throw new KeyNotFoundException($"Term with ID {updatedTerm.Id} not found");

            existingTerm.Term = updatedTerm.Term;
            existingTerm.Definition = updatedTerm.Definition;
            existingTerm.CreatedBy = updatedTerm.CreatedBy;
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

        public GlossaryTermDTO ArchiveTerm(int id, string currentUsername)
        {
            var existingTerm = _repositoryGlossary.GetById(id); ;
            if (existingTerm == null)
                throw new KeyNotFoundException($"Term with ID {id} not found");

            if (existingTerm.CreatedBy != currentUsername)
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

        public GlossaryTermDTO PublishTerm(GlossaryTermDTO updatedTerm)
        {
            var existingTerm = _repositoryGlossary.GetById(updatedTerm.Id); ;
            if (existingTerm == null)
                throw new KeyNotFoundException($"Term with ID {updatedTerm.Id} not found");

            _validator.ValidateTermForPublish(updatedTerm);


            existingTerm.Term = updatedTerm.Term;
            existingTerm.Definition = updatedTerm.Definition;
            existingTerm.CreatedBy = updatedTerm.CreatedBy;
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

        public void DeleteTerm(int id)
        {
            var term = _repositoryGlossary.GetById(id);
            if (term == null)
                throw new KeyNotFoundException($"Term with ID {id} not found");

            if (term.Status != ItemStatus.Draft)
                throw new InvalidOperationException("Only draft terms can be deleted");

            _repositoryGlossary.Delete(term);
            _repositoryGlossary.SaveChanges();
        }
    }

}
