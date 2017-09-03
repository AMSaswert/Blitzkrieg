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
using System.Web.Http.OData;

namespace BackEnd.Controllers
{
    public class VotesController : ApiController
    {
        private BAContext db = new BAContext();

        // GET: api/Votes
        /*public IQueryable<Vote> GetVotes()
        {
            return db.Votes;
        }

        // GET: api/Votes/5
        [ResponseType(typeof(Vote))]
        public IHttpActionResult GetVote(int id)
        {
            Vote vote = db.Votes.Find(id);
            if (vote == null)
            {
                return NotFound();
            }

            return Ok(vote);
        }
        */
        // PUT: api/Votes/5
        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVote(int id, Vote vote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vote.Id)
            {
                return BadRequest();
            }

            if (!db.Users.Any(u => u.UserName == vote.AuthorUsername))
            {
                return BadRequest("No user with that username");
            }

            if(vote.AuthorUsername != User.Identity.GetUserName())
            {
                return BadRequest("cant change other ppls vote");
            }

            if(vote.EntityType == EntityType.Subforum)
            {
                return BadRequest("Cant vote on subforum");
            }
            Vote vo = new Vote();
            if (vote.EntityType == EntityType.Topic)
            {
                if (!db.Topics.Any(t => t.Id == vote.EntityId))
                {
                    return BadRequest("No topic with that id");
                }
                var topic = db.Topics.Find(vote.EntityId);

                vo = db.Votes.Where(v => v.EntityType == EntityType.Topic &&
                                               v.AuthorUsername == vote.AuthorUsername &&
                                               v.EntityId == topic.Id).FirstOrDefault();
                if (vo == null)
                {
                    return BadRequest("Cant update non-existant vote");
                }

                if(vote.Liked && !vo.Liked)
                {
                    topic.LikesNo++;
                    topic.DislikesNo--;
                    db.Entry(topic).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else if(!vote.Liked && vo.Liked)
                {
                    topic.DislikesNo++;
                    topic.LikesNo--;
                    db.Entry(topic).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (vote.EntityType == EntityType.Comment)
            {
                if (!db.Comments.Any(c => c.Id == vote.EntityId))
                {
                    return BadRequest("No comment with that id");
                }

                var com = db.Comments.Find(vote.EntityId);

                vo = db.Votes.Where(v => v.EntityType == EntityType.Comment &&
                                               v.AuthorUsername == vote.AuthorUsername &&
                                               v.EntityId == com.Id).FirstOrDefault();
                if (vo == null)
                {
                    return BadRequest("Cant update non-existant vote");
                }

                if (vote.Liked && !vo.Liked)
                {
                    com.LikesNo++;
                    com.DislikesNo--;
                    db.Entry(com).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else if (!vote.Liked && vo.Liked)
                {
                    com.DislikesNo++;
                    com.LikesNo--;
                    db.Entry(com).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            if(vo.AuthorUsername != vote.AuthorUsername || vo.EntityId != vote.EntityId || vo.EntityType != vote.EntityType || vo.Id != vote.Id)
            {
                return BadRequest("Can only change if boolean Liked");
            }

            vo.Liked = vote.Liked;
            
            db.Entry(vo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoteExists(id))
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

        // POST: api/Votes
        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [ResponseType(typeof(Vote))]
        public IHttpActionResult PostVote(Vote vote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!db.Users.Any(u => u.UserName == vote.AuthorUsername))
            {
                return BadRequest("No user with that username");
            }

            if(vote.AuthorUsername != User.Identity.GetUserName())
            {
                return BadRequest("Cant post votes in name of other ppl");
            }

            if (vote.EntityType == EntityType.Subforum)
            {
                return BadRequest("Cant vote on subforum");
            }

            if (vote.EntityType == EntityType.Topic)
            {
                if (!db.Topics.Any(t => t.Id == vote.EntityId))
                {
                    return BadRequest("No topic with that id");
                }
                var topic = db.Topics.Find(vote.EntityId);

                var vo = db.Votes.Where(v => v.EntityType == EntityType.Topic &&
                                               v.AuthorUsername == vote.AuthorUsername &&
                                               v.EntityId == topic.Id).FirstOrDefault();
                if(vo != null)
                {
                    return BadRequest("You already voted");
                }

                if (vote.Liked)
                {
                    topic.LikesNo++;
                }
                else
                {
                    topic.DislikesNo++;
                }

                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
            }
            else if (vote.EntityType == EntityType.Comment)
            {
                if (!db.Comments.Any(c => c.Id == vote.EntityId))
                {
                    return BadRequest("No comment with that id");
                }

                var com = db.Comments.Find(vote.EntityId);

                var vo = db.Votes.Where(v => v.EntityType == EntityType.Comment &&
                                               v.AuthorUsername == vote.AuthorUsername &&
                                               v.EntityId == com.Id).FirstOrDefault();
                if (vo != null)
                {
                    return BadRequest("You already voted");
                }

                if(vote.Liked)
                {
                    com.LikesNo++;
                }else
                {
                    com.DislikesNo++;
                }

                db.Entry(com).State = EntityState.Modified;
                db.SaveChanges();
            }
            db.Votes.Add(vote);


            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = vote.Id }, vote);
        }

        // DELETE: api/Votes/5
        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [ResponseType(typeof(Vote))]
        public IHttpActionResult DeleteVote(int id)
        {
            Vote vote = db.Votes.Find(id);
            if (vote == null)
            {
                return NotFound();
            }

            if(vote.AuthorUsername != User.Identity.GetUserName())
            {
                return BadRequest("Cant delete other ppls votes");
            }
            
            if(vote.EntityType == EntityType.Topic)
            {
                var topic = db.Topics.Find(vote.EntityId);
                
                if(vote.Liked)
                {
                    topic.LikesNo--;
                }else
                {
                    topic.DislikesNo--;
                }
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
            }else if(vote.EntityType == EntityType.Comment)
            {
                var com = db.Comments.Find(vote.EntityId);

                if (vote.Liked)
                {
                    com.LikesNo--;
                }
                else
                {
                    com.DislikesNo--;
                }
                db.Entry(com).State = EntityState.Modified;
                db.SaveChanges();
            }

            db.Votes.Remove(vote);
            db.SaveChanges();

            return Ok(vote);
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Votes/GetTopicVote/{id}")]
        [ResponseType(typeof(Vote))]
        public IHttpActionResult GetTopicVote(int id)
        {
            var topic = db.Topics.Find(id);
            if(topic == null)
            {
                return BadRequest("No topic with that id");
            }

            var baUser = db.Users.Find(User.Identity.GetUserName());
            if(baUser == null)
            {
                return BadRequest("not logged in");
            }


            var vote = db.Votes.Where(v => v.EntityType == EntityType.Topic && 
                                           v.AuthorUsername == baUser.UserName &&
                                           v.EntityId == topic.Id).FirstOrDefault();

            if(vote == null)
            {
                return Ok<Vote>(null);
            }

            return Ok(vote);
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpGet]
        [Route("api/Votes/GetCommentVote/{id}")]
        [ResponseType(typeof(Vote))]
        public IHttpActionResult GetCommentVote(int id)
        {
            var comment = db.Comments.Find(id);
            if (comment == null)
            {
                return BadRequest("No comment with that id");
            }

            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return BadRequest("not logged in");
            }


            var vote = db.Votes.Where(v => v.EntityType == EntityType.Comment &&
                                           v.AuthorUsername == baUser.UserName &&
                                           v.EntityId == comment.Id).FirstOrDefault();

            if (vote == null)
            {
                return Ok<Vote>(null);
            }

            return Ok(vote);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VoteExists(int id)
        {
            return db.Votes.Count(e => e.Id == id) > 0;
        }
    }
}