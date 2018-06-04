using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APS.Controllers
{
    public class BlockedController : Controller
    {
        // GET: Blocked
        public ActionResult Index()
        {
            ViewBag.Blocked = true;
            return View();
        }
    }
}