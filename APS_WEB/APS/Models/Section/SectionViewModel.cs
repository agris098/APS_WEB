using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using System;

namespace APS.Models
{
    public class SectionViewModel
    {
        public ObjectId Id { get; set; }
        [BsonElement("Parent")]
        public string Parent { get; set; }
        [BsonElement("Child")]
        public string Child { get; set; }
        [BsonElement("Path")]
        public string Path { get; set; }
        [BsonElement("Columns")]
        public string[] Columns { get; set; }
        [BsonElement("Fields")]
        public string[] Fields { get; set; }

        public string ID { get { return Id.ToString(); }}
        public int Count { get; set; }

    }
}