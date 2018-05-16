using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APS.Models;
namespace APS.Controllers
{
    [Authorize(Roles = "Admin, Support")]
    [RoutePrefix("admin")]
    public class AdminController : Controller
    {
        private readonly DataAccess _objds;

        public AdminController() {
            _objds = new DataAccess();
        }

        [Route("")]
        public ActionResult Index()
        {
            ViewBag.AdministrationPage = true;
            return View();
        }
        [Authorize]
        [Route("addsection")]
        public ActionResult AddSection()
        {
            ViewBag.AdministrationPage = true;
            return View();
        }
        [Route("pclassifieds")]
        public ActionResult PClassifieds()
        {
            var classifieds = _objds.GetPublicedClassifieds();
            ViewBag.AdministrationPage = true;
            return View(classifieds);
        }
        [Route("work")]
        public ActionResult Work()
        {
            ViewBag.AdministrationPage = true;
            return View();
        }
        [Route("reports")]
        public ActionResult Reports()
        {
            ViewBag.AdministrationPage = true;
            var reports = _objds.ReportsGet();
            return View();
        }
    }
}