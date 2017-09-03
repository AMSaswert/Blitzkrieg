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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace BackEnd.Controllers
{
    public class SubforumModerationsController : ApiController
    {
        private BAContext db = new BAContext();

        // GET: api/SubforumModerations
        /*public IQueryable<SubforumModeration> GetSubforumModerations()
        {
            return db.SubforumModerations;
        }

        // GET: api/SubforumModerations/5
        [ResponseType(typeof(SubforumModeration))]
        public IHttpActionResult GetSubforumModeration(int id)
        {
            SubforumModeration subforumModeration = db.SubforumModerations.Find(id);
            if (subforumModeration == null)
            {
                return NotFound();
            }

            return Ok(subforumModeration);
        }

        // PUT: api/SubforumModerations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSubforumModeration(int id, SubforumModeration subforumModeration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subforumModeration.Id)
            {
                return BadRequest();
            }

            if (!db.Users.Any(u => u.UserName == subforumModeration.ModeratorUsername))
            {
                return BadRequest("wrong username");
            }

            db.Entry(subforumModeration).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubforumModerationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }*/

        // POST: api/SubforumModerations
        [Authorize(Roles = "Moderator, Admin")]
        [ResponseType(typeof(SubforumModeration))]
        public IHttpActionResult PostSubforumModeration(SubforumModeration subforumModeration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = db.Users.Find(User.Identity.GetUserName());
            if(user == null)
            {
                return BadRequest("Not logged in");
            }

            if (!db.Users.Any(u => u.UserName == subforumModeration.ModeratorUsername))
            {
                return BadRequest("wrong username");
            }

            if(!db.Subforums.Any(s => s.Id == subforumModeration.SubforumId))
            {
                return BadRequest("no subforum with that id");
            }

            if(db.SubforumModerations.Any(sm => sm.ModeratorUsername == subforumModeration.ModeratorUsername && sm.SubforumId == subforumModeration.SubforumId))
            {
                return BadRequest("user already moderates this subforum");
            }

            if(db.Subforums.Any(s=> s.LeadModeratorUsername == subforumModeration.ModeratorUsername && s.Id == subforumModeration.SubforumId))
            {
                return BadRequest("user is already lead moderator of this subforum");
            }

            db.SubforumModerations.Add(subforumModeration);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = subforumModeration.Id }, subforumModeration);
        }

        // DELETE: api/SubforumModerations/5
        /*[ResponseType(typeof(SubforumModeration))]
        public IHttpActionResult DeleteSubforumModeration(int id)
        {
            SubforumModeration subforumModeration = db.SubforumModerations.Find(id);
            if (subforumModeration == null)
            {
                return NotFound();
            }

            db.SubforumModerations.Remove(subforumModeration);
            db.SaveChanges();

            return Ok(subforumModeration);
        }*/


        [Authorize(Roles = "Moderator, Admin")]
        [HttpGet]
        [Route("api/SubforumModerations/GetModsNotAssigned/{subfId}")]
        [ResponseType(typeof(List<string>))]
        public IHttpActionResult GetModsNotAssigned(int subfId)
        {
            var loggedInUser = db.Users.Find(User.Identity.GetUserName());
            if(loggedInUser == null)
            {
                return BadRequest("Not logged in");
            }

            var subf = db.Subforums.Find(subfId);
            if(subf == null)
            {
                return BadRequest("no subforum with that id");
            }

            var userStore = new UserStore<BAIdentityUser>(db);
            var userManager = new UserManager<BAIdentityUser>(userStore);

            if(!userManager.IsInRole(loggedInUser.Id,"Admin") && subf.LeadModeratorUsername != loggedInUser.UserName)
            {
                return Unauthorized();
            }

            var users = db.Users.ToList();

            List<string> usernames = new List<string>();
            foreach(var us in users)
            {
                if(userManager.IsInRole(us.Id, "Moderator") && us.UserName != loggedInUser.UserName)
                {
                    if(!db.SubforumModerations.Any(sm => sm.ModeratorUsername == us.UserName && sm.SubforumId == subf.Id))
                    {
                        usernames.Add(us.UserName);
                    }
                }
            }

            return Ok(usernames);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubforumModerationExists(int id)
        {
            return db.SubforumModerations.Count(e => e.Id == id) > 0;
        }
    }
}