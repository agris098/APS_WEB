using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class ClassifieldModel
    {
        public ObjectId Id { get; set; }
        [BsonElement("SectionId")]
        public string SectionId { get; set; }
        [BsonElement("S_userId")]
        public string S_userId { get; set; }
        [BsonElement("S_price")]
        public decimal S_price { get; set; }
        [BsonElement("S_dateCreated")]
        public DateTime S_dateCreated { get; set; }
        [BsonElement("S_pictures")]
        public string[] S_pictures { get; set; }
        [BsonElement("S_description")]
        public string S_description { get; set; }
        [BsonElement("S_condition")]
        public string S_condition { get; set; }
        [BsonElement("S_endDate")]
        public DateTime S_endDate { get; set; }
        [BsonElement("Deleted")]
        public bool? Deleted { get; set; }
        [BsonElement("Status")]
        public Status Status { get; set; }
    }
}