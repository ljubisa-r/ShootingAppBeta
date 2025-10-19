using GlossaryAPI.DTOs;

namespace GlossaryAPI.Interfaces
{
    public interface IGlossaryService
    {
        IEnumerable<GlossaryTermDTO> GetAllTerms();
        GlossaryTermDTO? GetTermById(int id);
        GlossaryTermDTO CreateTerm(GlossaryTermDTO newTerm);
        GlossaryTermDTO UpdateTerm(GlossaryTermDTO updatedTerm);
        GlossaryTermDTO ArchiveTerm(int id, string currentUsername);
        GlossaryTermDTO PublishTerm(GlossaryTermDTO updatedTerm);
        void DeleteTerm(int id);
    }
}
