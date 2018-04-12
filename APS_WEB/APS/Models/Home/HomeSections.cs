using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class HomeSections
    {
        public SectionViewModel MainSection { get; set; }
        public List<SectionViewModel> Childs { get; set; }
    }
}