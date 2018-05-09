using APS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APS.Controllers.Account
{
    [Authorize]
    [RoutePrefix("profile")]
    public class ProfileController : Controller
    {
        DataAccess _objds;
        public ProfileController(){
            _objds = new DataAccess();
        }
        // GET: Profile
        [HttpGet]
        [Route("{id}")]
        public ActionResult Index(string id)
        {
            var user = _objds.GetUserDetailsFull(id);
            
            return View(user);
        }
        [HttpGet]
        [Route("edit")]
        public ActionResult Edit()
        {
            var id = HttpContext.User.Identity.GetUserId();
            var user = _objds.GetUserDetailsFull(id);

            return View(user);
        }
        [HttpGet]
        [Route("publicedclassifieds")]
        public ActionResult PublicedClassifiedsForUser(string id)
        {
            var classifieds = _objds.GetPublicedClassifiedsForUser(id);
            return Json(classifieds, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("update")]
        public ActionResult Update([Bind(Exclude = "UserPhoto")]UserDetails model)
        {
            string userId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {

                // To convert the user uploaded Photo as Byte Array before save to DB
                byte[] imageData = null;
                string thePictureDataAsString = "";
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase userImage = Request.Files["UserPhoto"];
                    /*
                    Image recivedImage = Image.FromStream(userImage.InputStream);
                    ImageManager IManager = new ImageManager(); 
                    Image lol = IManager.ResizeImage(recivedImage, 200, 200, System.Drawing.Imaging.ImageFormat.Jpeg);
                    */
                    using (var binary = new BinaryReader(userImage.InputStream))
                    {
                        imageData = binary.ReadBytes(userImage.ContentLength);
                    }
                    
                    thePictureDataAsString = Convert.ToBase64String(imageData);
                }
                _objds.UpdateUserDetails(userId,thePictureDataAsString, model);
            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Index", "Profile", new { id = userId});
        }
    }
}