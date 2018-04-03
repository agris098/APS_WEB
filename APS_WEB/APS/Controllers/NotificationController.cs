using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APS.Controllers
{
    [RoutePrefix("notifications")]
    public class NotificationController : Controller
    {
        // GET: Notification
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}