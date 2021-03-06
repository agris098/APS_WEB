﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APS.Models;

namespace APS.Controllers
{

    public class HomeController : Controller
    {
        DataAccess objds;

        public HomeController()
        {
            objds = new DataAccess();
        }
        public ActionResult Index()
        {
            var sections = objds.GetSections();
            var classifieds = objds.GetClassifieldAll();
            var groupList = new List<HomeSections>();

            var mainSections = sections.Where(s => s.Parent == "classifields");



            foreach (var s in mainSections) {
                var g = new SectionViewModel()
                {
                    Child = s.Child,
                    Parent = s.Parent,
                    Id = s.Id,
                    Columns = s.Columns,
                    Path = s.Path,
                    Fields = s.Fields,
                    Count = objds.ClassifieldCountByPath(s.Path + "/" +s.Child )
                };
                var gg = new HomeSections()
                {
                    MainSection = g,
                    Childs = sections.Where(ss => ss.Parent == g.Child).Select(p => new SectionViewModel()
                    {
                        Child = p.Child,
                        Parent = p.Parent,
                        Id = p.Id,
                        Columns = p.Columns,
                        Path = p.Path,
                        Fields = p.Fields,
                        Count = objds.ClassifieldCountByPath(p.Path + "/" + p.Child)
                    }).ToList()
                };
                groupList.Add(gg);
            }


            return View(groupList);
        }
    }
}