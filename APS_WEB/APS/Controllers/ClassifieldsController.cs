
using System.Web.Mvc;
using System.Collections.Generic;

using APS.Models;
using MongoDB.Bson;
using System.Linq;

namespace APS.Controllers
{

    [RoutePrefix("classifields")]
    public class ClassifieldsController : Controller
    {
        DataAccess objds;

       public ClassifieldsController() {
            objds = new DataAccess();
       }
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
                    var temp = objds.GetSectionsByPath(path);
                    var sections = temp.Select(s => new SectionViewModel()
                    {
                        Child = s.Child,
                        Parent = s.Parent,
                        Id = s.Id,
                        Columns = s.Columns,
                        Path = s.Path,
                        Fields = s.Fields,
                        Count = objds.ClassifieldCountByPath(s.Path + "/" + s.Child)
                    });
                    return View(sections);
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