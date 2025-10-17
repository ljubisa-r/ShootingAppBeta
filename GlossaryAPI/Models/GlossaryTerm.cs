
namespace GlossaryAPI.Models
{
    public class GlossaryTerm
    {
        public int Id { get; set; }
        public string Term { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public ItemStatus Status { get; set; } = ItemStatus.Draft;

    }
}
