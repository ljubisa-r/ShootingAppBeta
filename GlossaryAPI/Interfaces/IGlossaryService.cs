using GlossaryAPI.DTOs;

namespace GlossaryAPI.Interfaces
{
    public interface IGlossaryService
    {
        IEnumerable<GlossaryTermDTO> GetAllTerms();
        GlossaryTermDTO? GetTermById(int id);
        GlossaryTermDTO CreateTerm(GlossaryTermDTO newTerm, int userId);
        GlossaryTermDTO UpdateTerm(GlossaryTermDTO updatedTerm, int userId);
        GlossaryTermDTO ArchiveTerm(int id, int userId);
        GlossaryTermDTO PublishTerm(GlossaryTermDTO updatedTerm, int userId);
        void DeleteTerm(int id, int userId);
    }
}
