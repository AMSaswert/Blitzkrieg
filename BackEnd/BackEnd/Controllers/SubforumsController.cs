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
    public class SubforumsController : ApiController
    {
        private BAContext db = new BAContext();
        private object subsLock = new object();

        // GET: api/Subforums
        /*ublic IQueryable<Subforum> GetSubforums()
        {
            return db.Subforums;
        }

        // GET: api/Subforums/5
        [ResponseType(typeof(Subforum))]
        public IHttpActionResult GetSubforum(int id)
        {
            Subforum subforum = db.Subforums.Find(id);
            if (subforum == null)
            {
                return NotFound();
            }

            return Ok(subforum);
        }

        // PUT: api/Subforums/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSubforum(int id, Subforum subforum)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!db.Users.Any(u => u.UserName == subforum.LeadModeratorUsername))
            {
                return BadRequest("wrong username");
            }


            if (id != subforum.Id)
            {
                return BadRequest();
            }

            db.Entry(subforum).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubforumExists(id))
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
        */
        // POST: api/Subforums
        [Authorize(Roles="Moderator, Admin")]
        [ResponseType(typeof(Subforum))]
        public IHttpActionResult PostSubforum(Subforum subforum)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!db.Users.Any(u => u.UserName == subforum.LeadModeratorUsername))
            {
                return BadRequest("wrong username");
            }
            if(subforum.LeadModeratorUsername != User.Identity.GetUserName())
            {
                return BadRequest("cant make subforum in other persons name"); 
            }

            db.Subforums.Add(subforum);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = subforum.Id }, subforum);
        }

        // DELETE: api/Subforums/5
        [Authorize(Roles = "Moderator, Admin")]
        [ResponseType(typeof(Subforum))]
        public IHttpActionResult DeleteSubforum(int id)
        {
            Subforum subforum = db.Subforums.Find(id);
            if (subforum == null)
            {
                return NotFound();
            }

            var userStore = new UserStore<BAIdentityUser>(db);
            var userManager = new UserManager<BAIdentityUser>(userStore);

            var baUser = db.Users.Find(User.Identity.GetUserName());
            if(baUser == null)
            {
                return BadRequest("not logged in");
            }

            if (!userManager.IsInRole(baUser.Id, "Admin") && baUser.UserName != subforum.LeadModeratorUsername)
            {
                return BadRequest("cant delete someone elses subforum");
            }

            DeleteVotes(subforum);
            var sMods = db.SubforumModerations.Where(sm => sm.SubforumId == id);
            db.SubforumModerations.RemoveRange(sMods);

            db.Subforums.Remove(subforum);
            db.SaveChanges();

            return Ok(subforum);
        }


        private void DeleteVotes(Subforum s)
        {
            var topics = s.Topics.ToList();

            foreach (var t in topics)
            {
                if (t != null)
                {
                    DeleteVotes(t);
                }
            }
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
        [Route("api/Subforums/{id}/IsSubscribed")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult IsSubscribed(int id)
        {
            var s = db.Subforums.Find(id);
            if(s == null)
            {
                return BadRequest("No subforum with that id");
            }

            var user = db.Users.Find(User.Identity.GetUserName());
            if(user == null)
            {
                return InternalServerError();
            }

            var appUser = db.AppUsers.Find(user.AppUserId);
            if(appUser == null)
            {
                return InternalServerError();
            }

            var isSubscribed = appUser.BookmarkedSubforums.Any(sub => sub.Id == id);
            return Ok(isSubscribed);
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpPut]
        [Route("api/Subforums/{id}/Unsubscribe")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Unsubscribe(int id, Subforum subf)
        {
            bool ise = false;
            lock (subsLock)
            {

                var s = db.Subforums.Find(id);
                if (s == null)
                {
                    return BadRequest("No subforum with that id");
                }

                var user = db.Users.Find(User.Identity.GetUserName());
                if (user == null)
                {
                    return InternalServerError();
                }

                var appUser = db.AppUsers.Find(user.AppUserId);
                if (appUser == null)
                {
                    return InternalServerError();
                }

                var isSubscribed = appUser.BookmarkedSubforums.Any(sub => sub.Id == id) || s.UsersWhoBookmarkedThis.Any(u => u.Id == appUser.Id);
                if (!isSubscribed)
                {
                    return BadRequest("Cant unsubscribe because not subscribed");
                }

                s.UsersWhoBookmarkedThis.Remove(appUser);
                appUser.BookmarkedSubforums.Remove(s);
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    ise = true;
                }

            }
            if(ise)
            {
                return InternalServerError();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpPut]
        [Route("api/Subforums/{id}/Subscribe")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Subscribe(int id, Subforum subf)
        {
            bool ise = false;
            lock(subsLock)
            {
                var s = db.Subforums.Find(id);
                if (s == null)
                {
                    return BadRequest("No subforum with that id");
                }

                var user = db.Users.Find(User.Identity.GetUserName());
                if (user == null)
                {
                    return InternalServerError();
                }

                var appUser = db.AppUsers.Find(user.AppUserId);
                if (appUser == null)
                {
                    return InternalServerError();
                }

                var isSubscribed = appUser.BookmarkedSubforums.Any(sub => sub.Id == id) || s.UsersWhoBookmarkedThis.Any(u=>u.Id == appUser.Id);
                if (isSubscribed)
                {
                    return BadRequest("Already subscribed");
                }

                s.UsersWhoBookmarkedThis.Add(appUser);
                appUser.BookmarkedSubforums.Add(s);
                try
                {
                    db.SaveChanges();
                }catch
                {
                    ise = true;
                }
            }
            if(ise)
            {
                return InternalServerError();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }


        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Subforums/CountSubscribed")]
        [ResponseType(typeof(int))]
        public IHttpActionResult CountSubscribed()
        {
            var user = db.Users.Find(User.Identity.GetUserName());
            if (user == null)
            {
                return InternalServerError();
            }

            var appUser = db.AppUsers.Find(user.AppUserId);
            if (appUser == null)
            {
                return InternalServerError();
            }
            var list = appUser.BookmarkedSubforums.ToList();

            
            return Ok(list.Count);
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Subforums/GetSubscribed/{skipNo}/{takeNo}")]
        [ResponseType(typeof(IQueryable<Subforum>))]
        public IQueryable<Subforum> GetSubscribed(int skipNo, int takeNo)
        {
            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return null;
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);

            var subscribed = appUser.BookmarkedSubforums.OrderByDescending(t => t.Name).Skip(skipNo).Take(takeNo).ToList();

            List<Subforum> retVal = new List<Subforum>();

            foreach (var subf in subscribed)
            {
                retVal.Add(new Subforum()
                {
                    Name = subf.Name,
                    Description = subf.Description,
                    IconURL = subf.IconURL,
                    Id = subf.Id,
                    LeadModeratorUsername = subf.LeadModeratorUsername,
                    Rules = subf.Rules
                });
            }


            return retVal.AsQueryable<Subforum>();
        }

        [Authorize(Roles = "Moderator")]
        [HttpGet]
        [Route("api/Subforums/CountModeratorsSubforums")]
        [ResponseType(typeof(int))]
        public IHttpActionResult CountModeratorsSubforums()
        {
            var user = db.Users.Find(User.Identity.GetUserName());
            if (user == null)
            {
                return InternalServerError();
            }

            var appUser = db.AppUsers.Find(user.AppUserId);
            if (appUser == null)
            {
                return InternalServerError();
            }

            var leadCount = db.Subforums.Count(s => s.LeadModeratorUsername == user.UserName);
            var assistCount = db.SubforumModerations.Count(sm => sm.ModeratorUsername == user.UserName);
            


            return Ok(leadCount + assistCount);
        }

        [Authorize(Roles = "Moderator")]
        [HttpGet]
        [Route("api/Subforums/GetModeratorsSubforums/{skipNo}/{takeNo}")]
        [ResponseType(typeof(IQueryable<Subforum>))]
        public IQueryable<Subforum> GetModeratorsSubforums(int skipNo, int takeNo)
        {
            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return null;
            }
            var appUser = db.AppUsers.Find(baUser.AppUserId);
            if (appUser == null)
            {
                return null;
            }

            var leadSubs = db.Subforums.Where(s => s.LeadModeratorUsername == baUser.UserName).ToList();
            var assistSubMods = db.SubforumModerations.Where(sm => sm.ModeratorUsername == baUser.UserName).ToList();
            List<Subforum> assistSubs = new List<Subforum>();
            foreach(var sm in assistSubMods)
            {
                assistSubs.Add(db.Subforums.Where(s => s.Id == sm.SubforumId).FirstOrDefault());
            }

            var allSubs = leadSubs.Concat(assistSubs);
            var temp = allSubs.AsQueryable<Subforum>();
            allSubs = temp.OrderByDescending(t => t.Name).Skip(skipNo).Take(takeNo).ToList();

            List<Subforum> retVal = new List<Subforum>();

            foreach (var subf in allSubs)
            {
                retVal.Add(new Subforum()
                {
                    Name = subf.Name,
                    Description = subf.Description,
                    IconURL = subf.IconURL,
                    Id = subf.Id,
                    LeadModeratorUsername = subf.LeadModeratorUsername,
                    Rules = subf.Rules
                });
            }


            return retVal.AsQueryable<Subforum>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubforumExists(int id)
        {
            return db.Subforums.Count(e => e.Id == id) > 0;
        }
    }
}