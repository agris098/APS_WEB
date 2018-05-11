using System;
using System.Collections.Generic;
using System.Linq;
using APS.Models;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web;
using System.IO;

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
            var userId = User.Identity.GetUserId();
            var classifield = objds.GetClassifield(id);
            if (userId != classifield.S_userId && classifield.Status != Status.Public)
            {
                return RedirectToAction("Index","Home");
            }    
            var ipAdress = Request.UserHostAddress;

            return View(objds.GetClassifiedViewModel(id, ipAdress));
        }
        [Authorize]
        [Route("add")]
        public ActionResult Add()
        {
            return View();
        }
        [Authorize]
        [Route("classifieds")]
        public ActionResult Classifieds()
        {
            return View();
        }
      //  [Authorize]
        [Route("mark")]
        [HttpPost]
        public ActionResult Mark(MarkModel m )
        {
            var userId = User.Identity.GetUserId();
            var mark = objds.MarkClassified(m.Id, userId);
            return Json(mark, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [Route("addclassified")]
        [HttpPost]
        public ActionResult AddClassified([Bind(Exclude = "S_pictures")]ClassifieldModel model)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                // To convert the user uploaded Photo as Byte Array before save to DB
                List<string> images = new List<string>();
                if (Request.Files.Count > 0)
                {
                    for (int i=0; i < Request.Files.Count; i++)
                    {
                        var img = Request.Files[i];
                        byte[] imageData = null;
                        string thePictureDataAsString = "";
                        //HttpPostedFileBase userImage = Request.Files["S_pictures"];
                        /*
                        Image recivedImage = Image.FromStream(userImage.InputStream);
                        ImageManager IManager = new ImageManager(); 
                        Image lol = IManager.ResizeImage(recivedImage, 200, 200, System.Drawing.Imaging.ImageFormat.Jpeg);
                        */
                        using (var binary = new BinaryReader(img.InputStream))
                        {
                            imageData = binary.ReadBytes(img.ContentLength);
                        }

                        thePictureDataAsString = Convert.ToBase64String(imageData);
                        images.Add(thePictureDataAsString);
                    }
                }

                // _objds.UpdateUserDetails(userId, thePictureDataAsString, model);
                //ClassifieldModel c = new ClassifieldModel()
                //{
                //    S_userId = User.Identity.GetUserId(),
                //    S_dateCreated = DateTime.Now,
                //    S_description = model.S_description,
                //    S_price = c.S_price,
                //    S_condition = c.S_condition,
                //    SectionId = objds.GetSectionByPath(c.Path).ID,
                //    Status = Status.Draft
                //};
                model.S_userId = userId;
                model.S_pictures = images.ToArray();
                model.S_mpicture = images.FirstOrDefault();
                model.S_dateCreated = DateTime.Now;
                model.Status = Status.Draft;
                model.SectionId = objds.GetSectionByPath(model.Path.Substring(1)).ID;
                objds.CreateSClassifield(model);

                //return View("Classifieds");
                return View("Classifieds");
            }
            catch (Exception e)
            {
                return View("Classifieds");
            }
        }
        [Route("getassigned")]
        public ActionResult GetAssigned(string id)
        {
            var classified = objds.GetClassifiedViewModel(id);
            return Json(classified);
        }
    }
}