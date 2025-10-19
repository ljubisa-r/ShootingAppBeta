using GlossaryAPI.Data;
using GlossaryAPI.Models;
using GlossaryAPI.Interfaces;

namespace GlossaryAPI.Repositories
{
    public class GlossaryRepository : IGlossaryRepository
    {
        private readonly GlossaryDbContext _context;

        public GlossaryRepository(GlossaryDbContext context)
        {
            _context = context;
        }

        public IQueryable<GlossaryTerm> GetAll() => _context.GlossaryTerms;

        public GlossaryTerm? GetById(int id) => _context.GlossaryTerms.FirstOrDefault(x => x.Id == id);

        public void Add(GlossaryTerm term) => _context.GlossaryTerms.Add(term);

        public void Update(GlossaryTerm term) => _context.GlossaryTerms.Update(term);

        public void Delete(GlossaryTerm term) => _context.GlossaryTerms.Remove(term);

        public void SaveChanges() => _context.SaveChanges();
    }

}
