using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BackEnd.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BackEnd.Controllers
{
    public class TopicsController : ApiController
    {
        private BAContext db = new BAContext();

        // GET: api/Topics
        /*public IQueryable<Topic> GetTopics()
        {
            return db.Topics;
        }

        // GET: api/Topics/5
        [ResponseType(typeof(Topic))]
        public IHttpActionResult GetTopic(int id)
        {
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return NotFound();
            }

            return Ok(topic);
        }
        */
        // PUT: api/Topics/5
        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTopic(int id, Topic topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != topic.Id)
            {
                return BadRequest();
            }

            if (!db.Users.Any(u => u.UserName == topic.AuthorUsername))
            {
                return BadRequest("wrong username");
            }

            if(db.Topics.Any(t => t.Id != topic.Id && t.Name == topic.Name && t.SubforumId == topic.SubforumId))
            {
                return BadRequest("Topic with that name exists in that subforum");
            }

            var baUser = db.Users.Find(User.Identity.GetUserName());

            var userStore = new UserStore<BAIdentityUser>(db);
            var userManager = new UserManager<BAIdentityUser>(userStore);

            if(!userManager.IsInRole(baUser.Id, "Admin"))
            {
                if(userManager.IsInRole(baUser.Id, "AppUser") && topic.AuthorUsername != baUser.UserName)
                {
                    return BadRequest("cant change other ppls topics");
                }else if(userManager.IsInRole(baUser.Id, "Moderator"))
                {
                    var s = db.Subforums.Find(topic.SubforumId);
                    if(s.LeadModeratorUsername != baUser.UserName)
                    {
                        return BadRequest("cant change topic on a subforum you dont lead");
                    }
                }
            }

            db.Entry(topic).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(id))
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

        // POST: api/Topics
        /*[ResponseType(typeof(Topic))]
        public IHttpActionResult PostTopic(Topic topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!db.Users.Any(u => u.UserName == topic.AuthorUsername))
            {
                return BadRequest("wrong username");
            }

            db.Topics.Add(topic);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = topic.Id }, topic);
        }*/

        // DELETE: api/Topics/5
        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [ResponseType(typeof(Topic))]
        public IHttpActionResult DeleteTopic(int id)
        {
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return NotFound();
            }

            var baUser = db.Users.Find(User.Identity.GetUserName());

            var userStore = new UserStore<BAIdentityUser>(db);
            var userManager = new UserManager<BAIdentityUser>(userStore);

            if (!userManager.IsInRole(baUser.Id, "Admin"))
            {
                if (userManager.IsInRole(baUser.Id, "AppUser") && topic.AuthorUsername != baUser.UserName)
                {
                    return BadRequest("cant delete other ppls topics");
                }
                else if(userManager.IsInRole(baUser.Id, "Moderator"))
                {
                    var s = db.Subforums.Find(topic.SubforumId);
                    if (s.LeadModeratorUsername != baUser.UserName)
                    {
                        return BadRequest("cant delete topic on a subforum you dont lead");
                    }
                }
            }

            DeleteVotes(topic);

            db.Topics.Remove(topic);
            db.SaveChanges();

            return Ok(topic);
        }

        private void DeleteVotes(Topic t)
        {
            var comments = db.Comments.Where(c => c.TopicId == t.Id).ToList();

            foreach (var c in comments)
            {
                var cVotes = db.Votes.Where(v => v.EntityType == EntityType.Comment && v.EntityId == c.Id);
                db.Votes.RemoveRange(cVotes);
            }

            var tVotes = db.Votes.Where(v => v.EntityType == EntityType.Topic && v.EntityId == t.Id);
            db.Votes.RemoveRange(tVotes);
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Topics/{id}/IsAssistantMod")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult IsAssistantMod(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserName());
            if (user == null)
            {
                return BadRequest("Not logged in");
            }

            var t = db.Topics.Find(id);
            if (t == null)
            {
                return BadRequest("No topic with that id");
            }
            

            return Ok(db.SubforumModerations.Any(sm => sm.SubforumId == t.SubforumId && sm.ModeratorUsername == user.UserName));
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Topics/{id}/IsLeadMod")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult IsLeadMod(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserName());
            if(user == null)
            {
                return BadRequest("Not logged in");
            }

            var t = db.Topics.Find(id);
            if(t == null)
            {
                return BadRequest("No topic with that id");
            }

            var subf = db.Subforums.Find(t.SubforumId);

            return Ok(subf.LeadModeratorUsername == user.UserName);
        }


        [HttpGet]
        [Route("api/Topics/{id}/CountComments")]
        [ResponseType(typeof(int))]
        public IHttpActionResult CountComments(int id)
        {
            var t = db.Topics.Find(id);
            if (t == null)
            {
                return BadRequest("No topic with that id");
            }
            var count = db.Comments.Count(c => c.TopicId == id);
            return Ok(count);
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Topics/CountSaved")]
        [ResponseType(typeof(int))]
        public IHttpActionResult CountSaved()
        {
            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return null;
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);

            var saved = appUser.SavedTopics.ToList();

            return Ok(saved.Count);
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Topics/CountVotedOn")]
        [ResponseType(typeof(int))]
        public IHttpActionResult CountVotedOn()
        {
            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return null;
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);
            var votes = db.Votes.Where(v => v.AuthorUsername == baUser.UserName && v.EntityType == EntityType.Topic).ToList();

            return Ok(votes.Count);
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Topics/GetSaved/{skipNo}/{takeNo}")]
        [ResponseType(typeof(IQueryable<Topic>))]
        public IQueryable<Topic> GetSaved(int skipNo, int takeNo)
        {
            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return null;
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);

            var saved = appUser.SavedTopics.OrderByDescending(t => t.CreationDate).Skip(skipNo).Take(takeNo).ToList();

            List<Topic> retVal = new List<Topic>();

            foreach (var savedTopic in saved)
            {
                retVal.Add(new Topic()
                {
                    CreationDate = savedTopic.CreationDate,
                    Content = savedTopic.Content,
                    AuthorUsername = savedTopic.AuthorUsername,
                    DislikesNo = savedTopic.DislikesNo,
                    Id = savedTopic.Id,
                    LikesNo = savedTopic.LikesNo,
                    Name = savedTopic.Name,
                    TopicType = savedTopic.TopicType,
                    SubforumId = savedTopic.SubforumId
                });
            }

            return retVal.AsQueryable<Topic>();
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Topics/GetVotedOn/{skipNo}/{takeNo}")]
        [ResponseType(typeof(IQueryable<Topic>))]
        public IQueryable<Topic> GetVotedOn(int skipNo, int takeNo)
        {
            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return null;
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);
            var votes = db.Votes.Where(v => v.AuthorUsername == baUser.UserName && v.EntityType == EntityType.Topic).ToList();

            List<Topic> topics = new List<Topic>();

            foreach(var vote in votes)
            {
                var t = db.Topics.Find(vote.EntityId);
                topics.Add(t);
            }

            IQueryable<Topic> rootList = topics.AsQueryable<Topic>();
            rootList = rootList.OrderByDescending(c => c.CreationDate).Skip(skipNo).Take(takeNo);

            List<Topic> list = rootList.ToList();


            List<Topic> retVal = new List<Topic>();

            foreach (var item in list)
            {
                retVal.Add(new Topic()
                {
                    CreationDate = item.CreationDate,
                    Content = item.Content,
                    AuthorUsername = item.AuthorUsername,
                    DislikesNo = item.DislikesNo,
                    Id = item.Id,
                    LikesNo = item.LikesNo,
                    Name = item.Name,
                    TopicType = item.TopicType,
                    SubforumId = item.SubforumId
                });
            }

            return retVal.AsQueryable<Topic>();
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Topics/{id}/IsSaved")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult IsSaved(int id)
        {
            var topic = db.Topics.Find(id);
            if (topic == null)
            {
                return BadRequest("no topic with that id");
            }

            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return BadRequest("not logged in");
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);

            return Ok(appUser.SavedTopics.Any(t => t.Id == topic.Id));
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpPut]
        [Route("api/Topics/{id}/Save")]
        [ResponseType(typeof(Topic))]
        public IHttpActionResult Save(int id, Topic topic)
        {
            bool ise = false;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != topic.Id)
            {
                return BadRequest();
            }

            var top = db.Topics.Find(id);
            if (top == null)
            {
                return BadRequest("no topic with that id");
            }
            

            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return BadRequest("not logged in");
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);

            if (appUser.SavedTopics.Any(t => t.Id == topic.Id))
            {
                return BadRequest("already saved");
            }


            appUser.SavedTopics.Add(top);
            top.UsersWhoSavedThis.Add(appUser);

            try
            {
                db.SaveChanges();
            }
            catch
            {
                ise = true;
            }
        
            if(ise)
            {
                return InternalServerError();
               }
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpPut]
        [Route("api/Topics/{id}/Unsave")]
        [ResponseType(typeof(Topic))]
        public IHttpActionResult Unsave(int id, Topic topic)
        {
            bool ise = false;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != topic.Id)
            {
                return BadRequest();
            }

            var top = db.Topics.Find(id);
            if (top == null)
            {
                return BadRequest("no topic with that id");
            }


            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return BadRequest("not logged in");
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);

            if (!appUser.SavedTopics.Any(t => t.Id == topic.Id))
            {
                return BadRequest("not saved, cant unsave");
            }

            appUser.SavedTopics.Remove(top);
            top.UsersWhoSavedThis.Remove(appUser);

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

        private bool TopicExists(int id)
        {
            return db.Topics.Count(e => e.Id == id) > 0;
        }
    }
}