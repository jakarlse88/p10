﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Abarnathy.HistoryAPI.Models
{
    public class Note
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Contents")]
        public string Contents { get; set; }
    }
}