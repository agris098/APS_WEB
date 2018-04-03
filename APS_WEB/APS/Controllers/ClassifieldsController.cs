
using System.Web.Mvc;
using System.Collections.Generic;

using APS.Models;
using MongoDB.Bson;

namespace APS.Controllers
{

    [RoutePrefix("classifields")]
    public class ClassifieldsController : Controller
    {
        DataAccess objds;

       public ClassifieldsController() {
            objds = new DataAccess();
       }

        // GET: Section
     //   public ActionResult Index()
     //   {
      //      return View();
      //  }
        [Route("{*tags}")]
        public ActionResult Section(string tags)
        {
            var path = "";
            if (tags != null)
                path = tags;

            if (objds.ValidateSection(path))
            {
                if (objds.HasChildren(path))
                {
                    return View(objds.GetSectionsByPath(path));

                }
                else
                {
                    var Section = objds.GetSectionByPath(path);
                    return View("Classifields", Section);
                } 
            }
            else {
                return View("Index");
            }     
        }
    }
}