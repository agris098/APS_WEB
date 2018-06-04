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
    [Authorize(Roles = "Admin, Support")]
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
            var users = objds.GetUsersAdmin();
            return Ok(users);
        }
        [Authorize]
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
        [Route("addreport")]
        [HttpPost]
        public IHttpActionResult AddReportError([FromBody]ReportAdd re)
        {
            objds.ReportInsert(re.Id, re.Title, re.Description);
            return Ok();
        }
        [Route("reportmark")]
        [HttpPost]
        public IHttpActionResult ReportsMark([FromBody]IdModel im)
        {
            objds.ReportsMark(im.Id);
            return Ok();
        }
        [Route("reportmarkall")]
        [HttpPost]
        public IHttpActionResult ReportsMarkAll()
        {
            objds.ReportsMarkAll();
            return Ok();
        }
        [Route("reportdeletemarked")]
        [HttpPost]
        public IHttpActionResult ReportsDeleteAllMarked()
        {
            objds.ReportsDeleteAllMarked();
            return Ok();
        }
        [Route("addreporterror")]
        [HttpPost]
        public IHttpActionResult AddReportError([FromBody]ReportErrorModel re)
        {
            objds.ReportErrorInsert(re);
            return Ok();
        }
        [Route("reportserror")]
        [HttpGet]
        public IHttpActionResult ReportsError()
        {
            var er = objds.ReportsErrorGet();
            return Ok(er);
        }
        [Route("reporterrormark")]
        [HttpPost]
        public IHttpActionResult ReportsErrorMark([FromBody]IdModel im)
        {
            objds.ReportsErrorMark(im.Id);
            return Ok();
        }
        [Route("reporterrormarkall")]
        [HttpPost]
        public IHttpActionResult ReportsErrorMarkAll()
        {
            objds.ReportsErrorMarkAll();
            return Ok();
        }
        [Route("reporterrordeletemarked")]
        [HttpPost]
        public IHttpActionResult ReportsErrorDeleteAllMarked()
        {
            objds.ReportsErrorDeleteAllMarked();
            return Ok();
        }
        [Route("edituser")]
        [HttpPost]
        public IHttpActionResult EditUser([FromBody]UserEditModal ue)
        {
            objds.AdminEditUser(ue);
            return Ok();
        }
    }
}
