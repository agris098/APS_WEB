using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Globalization;

namespace APS.Controllers
{
    public class LanguageController : Controller
    {
        public ActionResult Change(String LanguageAttribute)
        {
            if (LanguageAttribute != null) {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAttribute);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAttribute);

                HttpCookie cookie = new HttpCookie("Language");
                cookie.Value = LanguageAttribute;
                Response.Cookies.Add(cookie); 
            }

            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
    }
}