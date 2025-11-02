using GlossaryAPI.Models;

namespace GlossaryAPI.Interfaces
{
    public interface IGlossaryRepository
    {
        IQueryable<GlossaryTerm> GetAll();
        GlossaryTerm? GetById(int id);
        IQueryable<GlossaryTerm> GetAllWithCreator();

        void Add(GlossaryTerm term);
        void Update(GlossaryTerm term);
        void Delete(GlossaryTerm term);
        void SaveChanges();
    }

}
