using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class ReportsG
    {
        public IEnumerable<ReportGroupModel> Reports  { get; set; }
        public IEnumerable<ReportErrorModel> ReportsError { get; set; }
    }
}