using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Abarnathy.HistoryService.Models
{
    /// <summary>
    /// Note entity.
    /// </summary>
    public class Note
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int PatientId { get; set; }
        
        [BsonElement("Contents")]
        public string Content { get; set; }

        public string Title { get; set; }
        
        public DateTime TimeCreated { get; set; }
        
        public DateTime TimeLastUpdated { get; set; }
    }
}