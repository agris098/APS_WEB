using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APS.Models;
using Microsoft.AspNet.Identity;

namespace APS.Controllers
{
    [RoutePrefix("api/classifields")]
    public class ClassifieldsApiController : ApiController
    {
        private readonly DataAccess objds;

        public ClassifieldsApiController()
        {
            objds = new DataAccess();
        }

        [Route("all/{id}")]
        [HttpGet]
        public IHttpActionResult GetClassifields(string id,[FromUri] Filter filter)
        {
            var classifields = objds.GetClassifieldsById(id);
            if (!string.IsNullOrEmpty(filter.Column) && !string.IsNullOrEmpty(filter.Order)) {
                if (filter.Order == "desc")
                {
                    classifields = classifields.OrderByDescending(s => s.GetType().GetProperty(filter.Column).GetValue(s, null));
                }
                else
                {
                    classifields = classifields.OrderBy(s => s.GetType().GetProperty(filter.Column).GetValue(s, null));
                }
            }
           // sections.OrderBy()
            return Ok(classifields);
        }
        [Route("add")]
        [HttpPost]
        public IHttpActionResult AddClassifield([FromBody]ClassifiedAddModel c)
        {
            try
            {
                ClassifieldModel n = new ClassifieldModel()
                {
                    S_userId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    S_dateCreated = DateTime.Now,
                    S_description = c.S_description,
                    S_price = c.S_price,
                    S_condition = c.S_condition,
                    SectionId = objds.GetSectionByPath(c.Path).ID,
                    Status = Status.Draft
                };
                objds.CreateSClassifield(n);

                return Ok();
            }
            catch(Exception e) {
                return InternalServerError(new Exception(e.Message));
            }
        }
        [Route("byuser")]
        [HttpGet]
        public IHttpActionResult GetClassifieldsByUserId()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var classifields = objds.GetClassifieldsByUserId(userId);

            List<MyClassifieds> mylist = new List<MyClassifieds>();

            MyClassifieds draft = new MyClassifieds();
            draft.Status = Status.Draft;
            draft.classifieds = classifields.Where(c => c.Status == Status.Draft).ToList();
            mylist.Add(draft);

            MyClassifieds publiced = new MyClassifieds();
            publiced.Status = Status.Public;
            publiced.classifieds = classifields.Where(c => c.Status == Status.Public).ToList();
            mylist.Add(publiced);

            MyClassifieds expired = new MyClassifieds();
            expired.Status = Status.Expired;
            expired.classifieds = classifields.Where(c => c.Status == Status.Expired).ToList();
            mylist.Add(expired);

            return Ok(mylist);
        }
        [Route("delete/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteClassifield(string id)
        {
            try
            {
                objds.DeleteClassified(id);

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(new Exception(e.Message));
            }
        }
        [Route("updatestatus")]
        [HttpPut]
        public IHttpActionResult UpdateClassifiedStatus(ClassifiedViewModel cv)
        {
            try
            {
                objds.UpdateClassifiedStatus(cv.Id, cv.Status);

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(new Exception(e.Message));
            }
        }
    }
}