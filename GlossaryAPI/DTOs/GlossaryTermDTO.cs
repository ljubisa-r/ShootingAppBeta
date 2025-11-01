﻿using GlossaryAPI.Models;
using System.Text.Json.Serialization;

namespace GlossaryAPI.DTOs
{
    public class GlossaryTermDTO
    {
        public int id { get; set; }
        public string term { get; set; } = string.Empty;
        public string definition { get; set; } = string.Empty;
        public int createdBy { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ItemStatus status { get; set; } = ItemStatus.Draft;

    }

}
