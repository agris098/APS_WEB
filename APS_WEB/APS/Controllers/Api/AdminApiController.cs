using APS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

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

        [Route("pclassifiedscount")]
        [HttpGet]
        public IHttpActionResult PublicedClassifieds()
        {
            var classifieds = objds.GetPublicedClassifiedsCount();
            return Ok(classifieds);
        }

        [Route("workerlist")]
        [HttpGet]
        public IHttpActionResult WorkerList()
        {
            var classifieds = objds.GetWorkerList();
            return Ok(classifieds);
        }
        [Route("assign")]
        [HttpPost]
        public IHttpActionResult Assign([FromBody]PClassifiedAssign a)
        {
            var count = objds.AssignClassifieds(a.Id, a.Count);
            return Ok(count);
        }
        [Route("workerinfo")]
        [HttpGet]
        public IHttpActionResult WorkerInfo()
        {

            var info = objds.GetWorkerInfo();
            return Ok(info);
        }
        [Route("workitem")]
        [HttpGet]
        public IHttpActionResult WorkerItem()
        {
            var id = User.Identity.GetUserId();
            var info = objds.GetWorkerItem(id);
            return Ok(info);
        }
        [Route("approveworkitem")]
        [HttpPut]
        public IHttpActionResult ApproveWorkItem([FromBody]ClassifiedViewModel c)
        {
            objds.ClassifiedApproveWorkItem(c.Id);
            return Ok();
        }
        [Route("rejectworkitem")]
        [HttpPut]
        public IHttpActionResult RejectWorkeItem([FromBody]ClassifiedViewModel c)
        {
            objds.ClassifiedRejectWorkItem(c.Id);
            return Ok();
        }
    }
}
