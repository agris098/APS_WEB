using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using APS.Models;

namespace APS.Extensions
{
    public static class HelperExtensions
    {

        public static CurrentUser CurrentUser(this HtmlHelper helper) {

            var userId =  HttpContext.Current.User.Identity.GetUserId();
            if (userId == null) {
                return new CurrentUser();
            }
            DataAccess db = new DataAccess();

            var user = db.GetUsers().Where(u => u.ID == userId).First();


            return new CurrentUser
            {
                Id = user.ID,
                Email = user.Email,
                ImageLarge = "",
                ImageSmall = "",
                FullName = user.UserName
            };
        }
    }
}