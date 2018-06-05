using System;
using System.Collections.Generic;
using System.Linq;
using APS.Models;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web;
using System.IO;
using System.Drawing;

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
        [Authorize]
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
                        //  byte[] imageData = null;
                        if (img.ContentLength != 0)
                        {
                            string thePictureDataAsString = "";
                            //HttpPostedFileBase userImage = Request.Files["S_pictures"];
                            /*
                            Image recivedImage = Image.FromStream(userImage.InputStream);
                            ImageManager IManager = new ImageManager(); 
                            Image lol = IManager.ResizeImage(recivedImage, 200, 200, System.Drawing.Imaging.ImageFormat.Jpeg);
                            */
                            Image recivedImage = Image.FromStream(img.InputStream);
                            ImageManager IManager = new ImageManager();
                            Image lol;
                            if (recivedImage.Width > 800)
                            {
                                var heigth = ((double)recivedImage.Height / recivedImage.Width) * 800;
                                lol = IManager.ResizeImage(recivedImage, 800, (int)heigth, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                            else
                            {
                                lol = recivedImage;
                            }

                            using (MemoryStream ms = new MemoryStream())
                            {
                                // Convert Image to byte[]
                                lol.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                byte[] imageBytes = ms.ToArray();

                                // Convert byte[] to Base64 String
                                thePictureDataAsString = Convert.ToBase64String(imageBytes);
                            }
                            images.Add(thePictureDataAsString);
                        }    
                    }
                }
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
        [Authorize(Roles = "Admin, Support")]
        [Route("getassigned")]
        public ActionResult GetAssigned(string id)
        {
            var classified = objds.GetClassifiedViewModel(id);
            return Json(classified);
        }
    }
}