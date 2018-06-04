using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class UserDetails
    {
        public ObjectId Id { get; set; }
        [BsonElement]
        public string UserId { get; set; }
        [BsonElement("FullName")]
        public string FullName { get; set; }
        [BsonElement("City")]
        public string City { get; set; }
        [BsonElement("Gender")]
        public string Gender { get; set; }
        [BsonElement("Skype")]
        public string Skype { get; set; }
        [BsonElement("WebAddress")]
        public string WebAddress { get; set; }
        [BsonElement("sm_image")]
        public string sm_image { get; set; }
        [BsonElement("lg_image")]
        public string lg_image { get; set; }
        [BsonElement("DOB")]
        public DateTime DOB { get; set; }
        [BsonElement("Nln")]
        public string NLn { get; set; }
        [BsonElement("Blocked")]
        public bool Blocked { get; set; }
    }
}