using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using BackEnd.Models;

namespace BackEnd.Controllers
{
    public class CommentsController : ApiController
    {
       // private BAContext db = new BAContext();

        // GET: api/Comments
        /*public IQueryable<Comment> GetComments()
        {
            return db.Comments;
        }

        // GET: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult GetComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }*/

        // PUT: api/Comments/5
        [Authorize(Roles ="AppUser, Moderator")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutComment(int id, Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var baUser = db.Users.Find(User.Identity.GetUserName());
            if(baUser == null)
            {
                return BadRequest("Not logged in");
            }

            if (id != comment.Id || !db.Users.Any(u => u.UserName == comment.AuthorUsername))
            {
                return BadRequest("wrong username");
            }

            if (!db.Comments.Any(c => c.Id == comment.Id))
            {
                return BadRequest("No comment with that id");
            }
            var comm = db.Comments.Find(comment.Id);
            if(comm.LikesNo != comment.LikesNo || comm.DislikesNo != comment.DislikesNo || comm.AuthorUsername != comment.AuthorUsername ||
               comm.CreationDate != comment.CreationDate || comm.ParentCommentId != comment.ParentCommentId || comm.TopicId != comment.TopicId ||
               comm.Edited != comment.Edited)
            {
                return BadRequest("you can only change text of a comment");
            }
            comm.Text = comment.Text;


            var userStore = new UserStore<BAIdentityUser>(db);
            var userManager = new UserManager<BAIdentityUser>(userStore);

            
            if (userManager.IsInRole(baUser.Id,"Moderator") && comment.AuthorUsername != baUser.UserName)
            {
                try
                {
                    var t = db.Topics.Find(comment.TopicId);
                    var s = db.Subforums.Find(t.SubforumId);
                    if(s.LeadModeratorUsername != baUser.UserName)
                    {
                        return BadRequest("Only author and lead moderator can change comment");
                    }
                    comm.Edited = false;
                }
                catch
                {
                    return BadRequest();
                }
            }else
            {
                if(comment.AuthorUsername != baUser.UserName)
                {
                    return BadRequest("Only author and lead moderator can change comment");
                }
                comm.Edited = true;
            }

            db.Entry(comm).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles ="AppUser, Moderator, Admin")]
        [HttpPut]
        [Route("api/Comments/{id}/Hide")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Hide(int id, Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return BadRequest("Not logged in");
            }

            if (id != comment.Id || !db.Users.Any(u => u.UserName == comment.AuthorUsername))
            {
                return BadRequest("wrong username");
            }

            if (!db.Comments.Any(c => c.Id == comment.Id))
            {
                return BadRequest("No comment with that id");
            }
            var comm = db.Comments.Find(comment.Id);
            if (comm.LikesNo != comment.LikesNo || comm.DislikesNo != comment.DislikesNo || comm.AuthorUsername != comment.AuthorUsername ||
               comm.CreationDate != comment.CreationDate || comm.ParentCommentId != comment.ParentCommentId || comm.TopicId != comment.TopicId ||
               comm.Edited != comment.Edited)
            {
                return BadRequest("you can only change text of a comment");
            }

            var userStore = new UserStore<BAIdentityUser>(db);
            var userManager = new UserManager<BAIdentityUser>(userStore);

            bool roleCheck = false;
            if(userManager.IsInRole(baUser.Id,"Admin"))
            {
                roleCheck = true;
            }else if(userManager.IsInRole(baUser.Id,"Moderator"))
            {
                try
                {
                    var t = db.Topics.Find(comment.TopicId);
                    var s = db.Subforums.Find(t.SubforumId);
                    if(s.LeadModeratorUsername == baUser.UserName)
                    {
                        roleCheck = true;
                    }else
                    {
                        roleCheck = db.SubforumModerations.Any(sm => sm.ModeratorUsername == baUser.UserName && sm.SubforumId == s.Id);
                    }
                }catch
                {
                    roleCheck = false;
                }
            }else if(userManager.IsInRole(baUser.Id, "AppUser"))
            {
                if(comment.AuthorUsername == baUser.UserName)
                {
                    roleCheck = true;
                }
            }

            if(!roleCheck)
            {
                return Unauthorized();
            }

            comm.RemovedByModerator = true;

            db.Entry(comm).State = EntityState.Modified;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Comments
        /*[ResponseType(typeof(Comment))]
        public IHttpActionResult PostComment(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!db.Users.Any(u => u.UserName == comment.AuthorUsername))
            {
                return BadRequest("wrong username");
            }

            db.Comments.Add(comment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = comment.Id }, comment);
        }*/

        // DELETE: api/Comments/5
        /*[ResponseType(typeof(Comment))]
        public IHttpActionResult DeleteComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            db.Comments.Remove(comment);
            db.SaveChanges();

            return Ok(comment);
        }*/

        [Authorize(Roles ="AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Comments/CountSaved")]
        [ResponseType(typeof(int))]
        public IHttpActionResult CountSaved()
        {
            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return null;
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);

            var saved = appUser.SavedComments.ToList();

            return Ok(saved.Count);
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Comments/CountVotedOn")]
        [ResponseType(typeof(int))]
        public IHttpActionResult CountVotedOn()
        {
            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return null;
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);
            var votes = db.Votes.Where(v => v.AuthorUsername == baUser.UserName && v.EntityType == EntityType.Comment).ToList();

            return Ok(votes.Count);
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Comments/GetSaved/{skipNo}/{takeNo}")]
        [ResponseType(typeof(IQueryable<Comment>))]
        public IQueryable<Comment> GetSaved(int skipNo, int takeNo)
        {
            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return null;
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);

            IQueryable<Comment> rootList = appUser.SavedComments.AsQueryable<Comment>();
            
            rootList = rootList.OrderByDescending(c => c.CreationDate).Skip(skipNo).Take(takeNo);

            var list = rootList.ToList();

            PrepareList(list);

            return list.AsQueryable<Comment>();
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Comments/GetVotedOn/{skipNo}/{takeNo}")]
        [ResponseType(typeof(IQueryable<Comment>))]
        public IQueryable<Comment> GetVotedOn(int skipNo, int takeNo)
        {
            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return null;
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);
            var votes = db.Votes.Where(v => v.AuthorUsername == baUser.UserName && v.EntityType == EntityType.Comment).ToList();

            List<Comment> retVal = new List<Comment>();

            foreach (var vote in votes)
            {
                var t = db.Comments.Find(vote.EntityId);
                retVal.Add(t);
            }
            IQueryable<Comment> rootList = retVal.AsQueryable<Comment>();

            rootList = rootList.OrderByDescending(c => c.CreationDate).Skip(skipNo).Take(takeNo);

            var list = rootList.ToList();

            PrepareList(list);

            return list.AsQueryable<Comment>();
        }


        [HttpGet]
        [Route("api/Comments/GetRootCommentsCount/{topicId}")]
        [ResponseType(typeof(int))]
        public IHttpActionResult GetRootCommentsCount(int topicId)
        {
            var topic = db.Topics.Find(topicId);
            if(topic == null)
            {
                return BadRequest("No topic with that id");
            }

            return Ok(db.Comments.Count(c => c.TopicId == topic.Id && c.ParentCommentId == null));
        }

        [HttpGet]
        [Route("api/Comments/GetCommentsCountByUsername/{username}")]
        [ResponseType(typeof(int))]
        public IHttpActionResult GetCommentsCountByUsername(string username)
        {
            var baUser = db.Users.Find(username);
            if (baUser == null)
            {
                return BadRequest("No user with that username");
            }

            return Ok(db.Comments.Count(c => c.AuthorUsername == username));
        }


        [HttpGet]
        [Route("api/Comments/GetPage/{topicId}/{skipNo}/{takeNo}")]
        [ResponseType(typeof(IQueryable<Comment>))]
        public IQueryable<Comment> GetPage(int topicId, int skipNo, int takeNo)
        {
            var topic = db.Topics.Find(topicId);
            if(topic == null)
            {
                return null;
            }

            IQueryable<Comment> rootList = db.Comments.Where(c => c.TopicId == topicId && c.ParentCommentId == null);


            rootList = rootList.OrderByDescending(c => c.CreationDate).Skip(skipNo).Take(takeNo);

            var list = rootList.ToList();

            PrepareList(list);

            return rootList.AsQueryable<Comment>();

        }

        [HttpGet]
        [Route("api/Comments/GetByUsername/{username}/{skipNo}/{takeNo}")]
        [ResponseType(typeof(IQueryable<Comment>))]
        public IQueryable<Comment> GetByUsername(string username, int skipNo, int takeNo)
        {
            var baUser = db.Users.Find(username);
            if (baUser == null)
            {
                return null;
            }

            IQueryable<Comment> rootList = db.Comments.Where(c => c.AuthorUsername == username);


            rootList = rootList.OrderByDescending(c => c.CreationDate).Skip(skipNo).Take(takeNo);

            var list = rootList.ToList();

            PrepareList(list);

            return rootList.AsQueryable<Comment>();

        }


        private void PrepareList(IEnumerable<Comment> list)
        {
            if(list == null || list.Count() == 0)
            {
                return;
            }

            foreach(var comm in list)
            {
                comm.ParentComment = null;
                comm.UsersWhoSavedThis = null;
                comm.Topic = null;

                IQueryable<Comment> childrenComments = db.Comments.Where(c => c.ParentCommentId == comm.Id);
                comm.ChildrenComments = childrenComments.OrderByDescending(c => c.CreationDate).ToList();

                if (comm.ChildrenComments != null && comm.ChildrenComments.Count() > 0)
                {
                    PrepareList(comm.ChildrenComments);
                }
            }
        }


        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Comments/{id}/IsSaved")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult IsSaved(int id)
        {
            var comment = db.Comments.Find(id);
            if (comment == null)
            {
                return BadRequest("no comment with that id");
            }

            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return BadRequest("not logged in");
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);

            return Ok(appUser.SavedComments.Any(t => t.Id == comment.Id));
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpPut]
        [Route("api/Comments/{id}/Save")]
        [ResponseType(typeof(Topic))]
        public IHttpActionResult Save(int id, Comment comment)
        {
            bool ise = false;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != comment.Id)
            {
                return BadRequest();
            }

            var comm = db.Comments.Find(id);
            if (comm == null)
            {
                return BadRequest("no comment with that id");
            }


            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return BadRequest("not logged in");
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);

            if (appUser.SavedComments.Any(c => c.Id == comment.Id))
            {
                return BadRequest("already saved");
            }

            appUser.SavedComments.Add(comm);
            comm.UsersWhoSavedThis.Add(appUser);

            try
            {
                db.SaveChanges();
            }
            catch
            {
                ise = true;
            }

            if (ise)
            {
                return InternalServerError();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpPut]
        [Route("api/Comments/{id}/Unsave")]
        [ResponseType(typeof(Topic))]
        public IHttpActionResult Unsave(int id, Comment comment)
        {
            bool ise = false;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != comment.Id)
            {
                return BadRequest();
            }

            var comm = db.Comments.Find(id);
            if (comm == null)
            {
                return BadRequest("no comment with that id");
            }


            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return BadRequest("not logged in");
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);

            if (!appUser.SavedComments.Any(c => c.Id == comment.Id))
            {
                return BadRequest("not saved, so cant unsave");
            }

            appUser.SavedComments.Remove(comm);
            comm.UsersWhoSavedThis.Remove(appUser);

            try
            {
                db.SaveChanges();
            }
            catch
            {
                ise = true;
            }

            if (ise)
            {
                return InternalServerError();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentExists(int id)
        {
            return db.Comments.Count(e => e.Id == id) > 0;
        }
    }
}