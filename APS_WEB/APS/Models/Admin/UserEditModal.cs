using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class UserEditModal
    {
        public string Id { get; set; }
        public bool Blocked { get; set; }
        public string Roles { get; set; }
    }
}