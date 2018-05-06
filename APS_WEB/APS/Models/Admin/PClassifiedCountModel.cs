using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class PClassifiedCountModel
    {
        public int Approved { get; set; }
        public int NotApproved { get; set; }
        public int Assigned { get; set; }
        public int NotAssigned { get; set; }

    }
}