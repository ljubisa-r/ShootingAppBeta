

namespace GlossaryAPI.Models
{
    public class GlossaryTerm
    {
        public int Id { get; set; }
        public string Term { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public int CreatedBy { get; set; } 
        public ItemStatus Status { get; set; } = ItemStatus.Draft;

        public User? Creator { get; set; }

    }
}
