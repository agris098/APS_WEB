using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class CurrentUser
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ImageSmall { get; set; }
        public string ImageLarge { get; set; }
        public bool Blocked { get; set; }
    }
}