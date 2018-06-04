using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class UserAdminModel
    {
        public String Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        public bool Blocked { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; } 
    }
}