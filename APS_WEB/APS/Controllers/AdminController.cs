using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APS.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("admin")]
    public class AdminController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            ViewBag.AdministrationPage = true;
            return View();
        }

        [Route("addsection")]
        public ActionResult AddSection()
        {
            ViewBag.AdministrationPage = true;
            return View();
        }
        [Route("pclassifieds")]
        public ActionResult PClassifieds()
        {
            ViewBag.AdministrationPage = true;
            return View();
        }
        [Route("work")]
        public ActionResult Work()
        {
            ViewBag.AdministrationPage = true;
            return View();
        }
    }
}