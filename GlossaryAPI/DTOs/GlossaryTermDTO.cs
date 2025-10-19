using GlossaryAPI.Models;
using System.Text.Json.Serialization;

namespace GlossaryAPI.DTOs
{
    public class GlossaryTermDTO
    {
        public int Id { get; set; }
        public string Term { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public ItemStatus Status { get; set; } = ItemStatus.Draft;


        public void ValidatePublishRequirements(GlossaryTermDTO term)
        {
            if (term == null)
                throw new ArgumentNullException(nameof(term), "Term cannot be null.");

            if (string.IsNullOrWhiteSpace(term.Term))
                throw new ArgumentException("Term name is required.", nameof(term.Term));

            if (string.IsNullOrWhiteSpace(term.Definition) || term.Definition.Length <= 30)
                throw new UnauthorizedAccessException("Definition must be longer than 30 characters.");

            if (term.Definition.Contains("test", StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Definition cannot contain the word 'test'.");
        }
    }

}
