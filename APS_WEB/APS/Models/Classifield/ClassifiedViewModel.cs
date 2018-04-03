using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class ClassifiedViewModel
    {
        public string Id { get; set; }
        public string SectionId { get; set; }
        public string S_userId { get; set; }
        public decimal S_price { get; set; }
        public DateTime S_dateCreated { get; set; }
        public string[] S_pictures { get; set; }
        public string S_description { get; set; }
        public string S_condition { get; set; }
        public DateTime S_endDate { get; set; }
        public Status Status { get; set; }
        public string U_email { get; set; }
        public string U_number { get; set; }
        public string U_location { get; set; }
    }
}