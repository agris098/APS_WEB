using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class StaticData
    {
        public ObjectId Id { get; set; }
        [BsonElement("Columns")]
        public string[] Columns { get; set; }
        [BsonElement("Fields")]
        public string[] Fields { get; set; }
    }
}