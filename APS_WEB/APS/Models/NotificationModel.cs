using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class NotificationModel
    {
        public ObjectId Id { get; set; }
        [BsonElement("UserId")]
        public string UserId { get; set; }
        [BsonElement("Description")]
        public string Description { get; set; }
        [BsonElement("Active")]
        public bool Active { get; set; }
        [BsonElement("Title")]
        public string Title { get; set; }
        [BsonElement("DateTime")]
        public DateTime DateTime { get; set; }

    }
}