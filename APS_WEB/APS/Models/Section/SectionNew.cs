using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class SectionNew
    {
        public string Id { get; set; }
        public string Child { get; set; }
        public string[] Columns { get; set; }
        public string[] Fields { get; set; }
    }
}