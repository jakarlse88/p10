using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Abarnathy.HistoryService.Models
{
    /// <summary>
    /// Note entity.
    /// </summary>
    public class Note
    {
        public Note()
        {
            NoteLog = new List<NoteLogItem>();
        }
        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("PatientId")]
        public int PatientId { get; set; }
        
        [BsonElement("Contents")]
        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }
        
        [JsonProperty("TimeCreated")]
        public DateTime TimeCreated { get; set; }
        
        [JsonProperty("TimeLastUpdated")]
        public DateTime TimeLastUpdated { get; set; }
        
        [JsonProperty("NoteLog")]
        public List<NoteLogItem> NoteLog { get; set; }
    }
}