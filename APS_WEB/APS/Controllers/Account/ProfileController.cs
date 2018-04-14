using APS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APS.Controllers.Account
{

    [RoutePrefix("profile")]
    public class ProfileController : Controller
    {
        DataAccess _objds;
        public ProfileController(){
            _objds = new DataAccess();
        }
        // GET: Profile
        [Route("{id}")]
        public ActionResult Index(string id)
        {
            var user = _objds.GetUser(id);
            
            return View(user);
        }
    }
}