using GlossaryAPI.Models;

namespace GlossaryAPI.Interfaces
{
    public interface IGlossaryRepository
    {
        IQueryable<GlossaryTerm> GetAll();
        GlossaryTerm? GetById(int id);
        void Add(GlossaryTerm term);
        void Update(GlossaryTerm term);
        void Delete(GlossaryTerm term);
        void SaveChanges();
    }

}
