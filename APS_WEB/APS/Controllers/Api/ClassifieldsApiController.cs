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

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetClassifield(string id)
        {
            var classifield = objds.GetClassifield(id);
            // sections.OrderBy()
            return Ok(classifield);
        }

        [Route("all/{id}")]
        [HttpGet]
        public IHttpActionResult GetClassifields(string id)
        {
            var classifields = objds.GetClassifieldsById(id);
           // sections.OrderBy()
            return Ok(classifields);
        }
        [Route("allpublished/{id}")]
        [HttpGet]
        public IHttpActionResult GetClassifieldsPublished(string id)
        {
            var classifields = objds.GetClassifieldsPublishedById(id);
            // sections.OrderBy()
            return Ok(classifields);
        }
        [Authorize]
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
        [Authorize]
        [Route("byuser")]
        [HttpGet]
        public IHttpActionResult GetClassifieldsByUserId()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var classifields = objds.GetClassifieldsByUserId(userId).Select(s =>  new MyClassifiedsModel() {
                Id = s.Id.ToString(),
                Description = s.S_description,
                Picture = s.S_mpicture,
                Price = s.S_price,
                Status = s.Status,
                Marks = s.Marks
            });

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

            MyClassifieds rejected = new MyClassifieds();
            rejected.Status = Status.Rejected;
            rejected.classifieds = classifields.Where(c => c.Status == Status.Rejected).ToList();
            mylist.Add(rejected);

            MyClassifieds marked = new MyClassifieds();
            marked.Status = Status.Public;
            marked.classifieds = objds.GetMarkedClassifiedsByUser(userId).Select(s => new MyClassifiedsModel()
                                {
                                    Id = s.Id.ToString(),
                                    Description = s.S_description,
                                    Picture = s.S_mpicture,
                                    Price = s.S_price,
                                    Status = s.Status,
                                    Marks = s.Marks
                                }).ToList();
            mylist.Add(marked);
            return Ok(mylist);
        }
        [Authorize]
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
        [Authorize]
        [Route("updatestatus")]
        [HttpPut]
        public IHttpActionResult UpdateClassifiedStatus(ClassifiedViewModel cv)
        {
            try
            {
                objds.UpdateClassifiedStatus(cv.Id, cv.Status, cv.Weeks);

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(new Exception(e.Message));
            }
        }
        [Route("comments/{id}")]
        [HttpGet]
        public IHttpActionResult GetClassifiedComments(string id) {
            try
            {
                var comments = objds.GetClassifiedComments(id);

                return Ok(comments);
            }
            catch (Exception e)
            {
                return InternalServerError(new Exception(e.Message));
            }
        }
        [Authorize]
        [Route("comments/add")]
        [HttpPost]
        public IHttpActionResult AddClassifiedComments([FromBody] CommentNew newC)
          {
            try
            {
                  var comment = objds.AddClassifiedComment(newC);

                  return Ok(comment);
            }
            catch (Exception e)
            {
                return InternalServerError(new Exception(e.Message));
            }
        }
        [Authorize]
        [Route("comments/like")]
        [HttpPost]
        public IHttpActionResult AddClassifiedCommentsLike([FromBody] CommentLike newL)
        {
            try
            {
                var response = objds.AddClassifiedCommentLike(newL);

                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalServerError(new Exception(e.Message));
            }
        }
    }
}