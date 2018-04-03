using APS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MongoDB.Driver;

namespace APS.Controllers.Api
{
    [RoutePrefix("api/admin")]
    public class AdminApiController : ApiController
    {
        private readonly DataAccess objds;

        public AdminApiController()
        {
            objds = new DataAccess();
        }

        [Route("users")]
        [HttpGet]
        public IHttpActionResult UsersList() {
            var users = objds.GetUsers();
            return Ok(users);
        }
        [Route("pclassifieds")]
        [HttpGet]
        public IHttpActionResult PublicedClassifieds()
        {
            var classifieds = objds.GetPublicedClassifieds();
            return Ok(classifieds);
        }
    }
}
