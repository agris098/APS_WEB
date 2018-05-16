using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace APS.Models
{
    public class ReportModel
    {
        public ObjectId Id { get; set; }
        [BsonElement("ClassifiedId")]
        public string ClassifiedId { get; set; }
        [BsonElement("Description")]
        public string Description { get; set; }
        [BsonElement("Active")]
        public bool Active { get; set; }
        [BsonElement("Title")]
        public string Title { get; set; }
        [BsonElement("DateTime")]
        public DateTime DateTime { get; set; }
    }
    public class ReportGroupModel
    {
        public string Id { get; set; }
        public List<ReportModel> items { get; set; }
    }
}