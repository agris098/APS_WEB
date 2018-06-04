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

            var user = db.GetUserDetailsFull(userId);

            return new CurrentUser
            {
                Id = user.UserId,
                Email = user.Email,
                ImageLarge = user.lg_image,
                ImageSmall = user.sm_image,
                FullName = user.FullName,
                Blocked = user.Blocked || false
            };
        }
    }
}