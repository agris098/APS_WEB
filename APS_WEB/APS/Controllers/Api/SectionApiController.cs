using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APS.Models;
using MongoDB.Bson;

namespace APS.Controllers
{
    [RoutePrefix("api/section")]
    public class SectionApiController : ApiController
    {
     private readonly DataAccess objds;

        public SectionApiController() {

            objds = new DataAccess();
        }

        //[Route("get")]
        //public IHttpActionResult Get(ObjectId id) {

        //    return Ok(objds.GetSection(id));
        //}

        [Route("getall/parent/{parent}")]
        [HttpGet]
        public IHttpActionResult GetAllByParent(string parent="")
        {
            if (parent == "classifields")
            {
                return Ok(objds.GetSectionsByPath(""));
            }
            return Ok(objds.GetSectionsByParent(parent));
        }
        [Route("staticdata")]
        [HttpGet]
        public IHttpActionResult GetStaticData()
        {
            return Ok(objds.GetStaticData());
        }

        [Route("haschildren/path")]
        [HttpPost]
        public IHttpActionResult HasChildren(SectionPath p)
        {
            return Ok(objds.HasChildren(p.Path));
        }

        [Route("getall")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return Ok(objds.GetSections());
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteSection(string Id)
        {
            objds.DeleteSection(Id);
            return Ok();
        }

        [Route("add")]
        [HttpPost]
        public IHttpActionResult AddSection([FromBody]SectionNew s)
        {
            objds.AddSection(s);
            return Ok();
        }
        [Route("getbypath")]
        [HttpPost]
        public IHttpActionResult GetByPath(SectionPath s)
        {
            var sectionFields = objds.GetSectionByPath(s.Path.Substring(1)).Fields;
            return Ok(sectionFields);
        }
    }
}
