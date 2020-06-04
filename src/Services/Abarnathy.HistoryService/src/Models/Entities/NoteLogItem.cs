using System;
using Newtonsoft.Json;

namespace Abarnathy.HistoryService.Models
{
    public class NoteLogItem
    {
        [JsonProperty("TimeOriginallyCreated")]
        public DateTime TimeOriginallyCreated { get; set; }
        
        [JsonProperty("TimeArchived")]
        public DateTime TimeArchived { get; set; }
        
        [JsonProperty("Title")]
        public string Title { get; set; }
        
        [JsonProperty("Content")]
        public string Content { get; set; }
    }
}