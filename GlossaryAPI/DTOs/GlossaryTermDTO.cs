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

    }

}
