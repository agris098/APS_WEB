using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class MyClassifieds
    {
        public Status Status { get; set; }
        public List<MyClassifiedsModel> classifieds {get;set;}
    }
}