using System;
using System.Collections.Generic;
using System.Linq;
using APS.Models;
using System.Web.Mvc;

namespace APS.Controllers
{

    [RoutePrefix("classifield")]
    public class ClassifieldController : Controller
    {
        DataAccess objds;

        public ClassifieldController()
        {
            objds = new DataAccess();
        }

        [Route("{id}")]
        public ActionResult Index(string id)
        {
            var ipAdress = Request.UserHostAddress;

            return View(objds.GetClassifiedViewModel(id, ipAdress));
        }
        [Authorize]
        [Route("addclassifield")]
        public ActionResult AddClassifield()
        {
            return View();
        }
        [Authorize]
        [Route("classifieds")]
        public ActionResult Classifieds()
        {
            return View();
        }

    }
}