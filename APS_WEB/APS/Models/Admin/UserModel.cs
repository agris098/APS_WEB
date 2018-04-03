using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    [BsonIgnoreExtraElements]
    public class UserModel
    {
        public ObjectId Id { get; set; }
        [BsonElement("UserName")]
        public string UserName { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }
        [BsonElement("EmailConfirmed")]
        public bool EmailConfirmed { get; set; }
        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; }
        [BsonElement("PhoneNumberConfirmed")]
        public bool PhoneNumberConfirmed { get; set; }
        [BsonElement("Roles")]
        public string[] Roles { get; set; }

        public string ID { get { return Id.ToString(); } }

        public string ROLES { get { return String.Join(",",this.Roles); } }
    }
}