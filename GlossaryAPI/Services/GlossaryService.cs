using GlossaryAPI.Data;
using GlossaryAPI.DTOs;
using GlossaryAPI.Interfaces;
using GlossaryAPI.Models;


namespace GlossaryAPI.Services
{
    public class GlossaryService : IGlossaryService
    {
        private readonly GlossaryDbContext _context;

        public GlossaryService(GlossaryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<GlossaryTermDTO> GetAllTerms()
        {

            var items = _context.GlossaryTerms;

            var dtos = items.Select(item => new GlossaryTermDTO
            {
                Id = item.Id,
                Term = item.Term,
                Definition = item.Definition,
                Status = item.Status                
            });

            return dtos;
        }

        public GlossaryTermDTO? GetTermById(int id)
        {

            var term = _context.GlossaryTerms.FirstOrDefault(t => t.Id == id);
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
                CreatedBy = newTermDto.CreatedBy ?? "Unknown",
                Status = ItemStatus.Draft
            };

            _context.GlossaryTerms.Add(newTerm);
            _context.SaveChanges();
            
            newTermDto.Id = newTerm.Id;
            newTermDto.Status = newTerm.Status;

            return newTermDto;
        }

        public GlossaryTermDTO UpdateTerm(GlossaryTermDTO updatedTerm)
        {
            var existingTerm = _context.GlossaryTerms.FirstOrDefault(t => t.Id == updatedTerm.Id);
            if (existingTerm == null)
                throw new KeyNotFoundException($"Term with ID {updatedTerm.Id} not found");

            existingTerm.Term = updatedTerm.Term;
            existingTerm.Definition = updatedTerm.Definition;
            existingTerm.CreatedBy = updatedTerm.CreatedBy;
            existingTerm.Status = updatedTerm.Status;

            _context.SaveChanges();
            
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
            var existingTerm = _context.GlossaryTerms.FirstOrDefault(t => t.Id == id);
            if (existingTerm == null)
                throw new KeyNotFoundException($"Term with ID {id} not found");

            if (existingTerm.CreatedBy != currentUsername)
                throw new UnauthorizedAccessException("You can only archive terms created by you");

            existingTerm.Status = ItemStatus.Archived;
            _context.SaveChanges();

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
            var term = _context.GlossaryTerms.FirstOrDefault(t => t.Id == id);
            if (term == null)
                throw new KeyNotFoundException($"Term with ID {id} not found");

            if (term.Status != ItemStatus.Draft)
                throw new InvalidOperationException("Only draft terms can be deleted");

            _context.GlossaryTerms.Remove(term);
            _context.SaveChanges();
        }
    }

}
