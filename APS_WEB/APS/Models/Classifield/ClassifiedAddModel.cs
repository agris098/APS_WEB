using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class ClassifiedAddModel
    {
        public decimal S_price { get; set; }

        public string[] S_pictures { get; set; }

        public string S_description { get; set; }

        public string S_condition { get; set; }

        public string Path { get; set; }
    }
}