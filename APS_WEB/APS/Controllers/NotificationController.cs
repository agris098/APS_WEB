using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APS.Models;
using Microsoft.AspNet.Identity;

namespace APS.Controllers
{
    [Authorize]
    [RoutePrefix("notifications")]
    public class NotificationController : Controller
    {
        DataAccess _objds;
        public NotificationController(){
            _objds = new DataAccess();
        }
        // GET: Notification
        [Route("")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var notifications = _objds.GetNotifications(userId);
            return View(notifications);
        }
        [Route("updatestatus")]
        [HttpPost]
        public ActionResult NotificationUpdateStatus(IdModel m ) {
            _objds.NotificationUpdateStatus(m.Id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}