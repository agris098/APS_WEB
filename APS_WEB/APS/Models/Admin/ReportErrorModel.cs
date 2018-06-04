using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APS.Models
{
    public class ReportErrorModel
    {
        public ObjectId Id { get; set; }
        [BsonElement("Description")]
        public string Description { get; set; }
        [BsonElement("Active")]
        public bool Active { get; set; }
        [BsonElement("DateTime")]
        public DateTime DateTime { get; set; }
    }
}