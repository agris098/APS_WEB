using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class ChatMessageModel
    {
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string ToUserId { get; set; }
        public string ToUserName { get; set; }
        public string Message { get; set; }
        public DateTime Created {get;set;}

        public string DateTime { get { return Created.ToString("dd/MM/yyyy HH:mm:ss"); } }
    }
}