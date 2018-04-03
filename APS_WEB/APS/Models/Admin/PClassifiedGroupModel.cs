using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class PClassifiedGroupModel
    {
        public IEnumerable<ClassifieldModel> NotApproved { get; set; }

        public IEnumerable<ClassifieldModel> Approved { get; set; }
    }
}