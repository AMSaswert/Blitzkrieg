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
    public class ComplaintsController : ApiController
    {
        private BAContext db = new BAContext();

        // GET: api/Complaints
        /* public IQueryable<Complaint> GetComplaints()
         {
            return db.Complaints;
         } */

        // GET: api/Complaints/5
        [Authorize(Roles ="Moderator, Admin")]
        [ResponseType(typeof(Complaint))]
        public IHttpActionResult GetComplaint(int id)
        {
            Complaint complaint = db.Complaints.Find(id);
            if (complaint == null)
            {
                return NotFound();
            }

            return Ok(complaint);
        }

        // PUT: api/Complaints/5
        /*[ResponseType(typeof(void))]
        public IHttpActionResult PutComplaint(int id, Complaint complaint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != complaint.Id)
            {
                return BadRequest();
            }

            if (!db.Users.Any(u => u.UserName == complaint.AuthorUsername))
            {
                return BadRequest("wrong username");
            }

            if (complaint.EntityType == EntityType.Subforum)
            {
                if (!db.Subforums.Any(s => s.Id == complaint.EntityId))
                {
                    return BadRequest("No subforum with that id");
                }
            }
            else if (complaint.EntityType == EntityType.Topic)
            {
                if (!db.Topics.Any(t => t.Id == complaint.EntityId))
                {
                    return BadRequest("No topic with that id");
                }
            }
            else if (complaint.EntityType == EntityType.Comment)
            {
                if (!db.Comments.Any(c => c.Id == complaint.EntityId))
                {
                    return BadRequest("No comment with that id");
                }
            }

            db.Entry(complaint).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComplaintExists(id))
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

        // POST: api/Complaints
        /*[ResponseType(typeof(Complaint))]
        public IHttpActionResult PostComplaint(Complaint complaint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!db.Users.Any(u => u.UserName == complaint.AuthorUsername))
            {
                return BadRequest("wrong username");
            }

            if (complaint.EntityType == EntityType.Subforum)
            {
                if (!db.Subforums.Any(s => s.Id == complaint.EntityId))
                {
                    return BadRequest("No subforum with that id");
                }
            }
            else if (complaint.EntityType == EntityType.Topic)
            {
                if (!db.Topics.Any(t => t.Id == complaint.EntityId))
                {
                    return BadRequest("No topic with that id");
                }
            }
            else if (complaint.EntityType == EntityType.Comment)
            {
                if (!db.Comments.Any(c => c.Id == complaint.EntityId))
                {
                    return BadRequest("No comment with that id");
                }
            }

            db.Complaints.Add(complaint);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = complaint.Id }, complaint);
        }
        */
        // DELETE: api/Complaints/5
        /*[ResponseType(typeof(Complaint))]
        public IHttpActionResult DeleteComplaint(int id)
        {
            Complaint complaint = db.Complaints.Find(id);
            if (complaint == null)
            {
                return NotFound();
            }

            db.Complaints.Remove(complaint);
            db.SaveChanges();

            return Ok(complaint);
        }*/

        private void SendMessage(string sender, string username, string text)
        {
            bool sd = true;
            if(sender == username)
            {
                sd = false;
            }

            Message m = new Message()
            {
                Content = text,
                CreationDate = DateTime.Now,
                Subject = "Report regulation",
                RecipientUsername = username,
                SenderUsername = sender,
                SenderDeleted = sd,
                SenderDeletedForReal = sd
            };
            db.Messages.Add(m);
            db.SaveChanges();
        }

        private List<string> GetSnitches(IQueryable<Complaint> complaints)
        {
            List<string> snitches = new List<string>();

            foreach(var item in complaints)
            {
                snitches.Add(item.AuthorUsername);
            }

            return snitches;
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpPut]
        [Route("api/Complaints/{id}/DeleteEntity")]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteEntity(int id, Complaint complaint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != complaint.Id)
            {
                return BadRequest();
            }

            var com = db.Complaints.Find(id);
            if(com == null)
            {
                return BadRequest("no complaint with that id");
            }

            var baUser = db.Users.Find(User.Identity.GetUserName());
            if(baUser == null)
            {
                return BadRequest("Not logged in");
            }

            var userStore = new UserStore<BAIdentityUser>(db);
            var userManager = new UserManager<BAIdentityUser>(userStore);

            if(!userManager.IsInRole(baUser.Id,"Admin"))
            {
                List<Complaint> inp = new List<Complaint>();
                inp.Add(complaint);
                List<Complaint> ret = FilterOutNotMine(inp, baUser.UserName);

                if (ret == null || ret.Count == 0)
                {
                    return Unauthorized();
                }
            }
            

            string messageForAuthor = "";
            string messageForSnitches = "";

            if (com.EntityType == EntityType.Subforum)
            {
                var subf = db.Subforums.Find(com.EntityId);
                if(subf == null)
                {
                    return BadRequest("No subforum with that id");
                }

                DeleteVotes(subf);

                var sMods = db.SubforumModerations.Where(sm => sm.SubforumId == subf.Id);
                db.SubforumModerations.RemoveRange(sMods);

                db.Subforums.Remove(subf);
                db.SaveChanges();
                messageForAuthor = "We have received complaint(s) about your subforum: \'" + subf.Name + "\' and after careful consideration we have decided to delete it.";
                messageForSnitches = "We have recieved your complaint about subforum: \'" + subf.Name + "\' and after careful consideration we have decided to delete it.";

                SendMessage(baUser.UserName, subf.LeadModeratorUsername, messageForAuthor);
            }
            else if(com.EntityType == EntityType.Topic)
            {
                var top = db.Topics.Find(com.EntityId);
                if(top == null)
                {
                    return BadRequest("No topic with that id");
                }

                DeleteVotes(top);

                db.Topics.Remove(top);
                db.SaveChanges();
                messageForAuthor = "We have received complaint(s) about your topic: \'" + top.Name + "\' and after careful consideration we have decided to delete it.";
                messageForSnitches = "We have received your complaint about topic: \'" + top.Name + "\' and after careful consideration we have decided to delete it.";

                SendMessage(baUser.UserName, top.AuthorUsername, messageForAuthor);
            }
            else if (com.EntityType == EntityType.Comment)
            {
                var comm = db.Comments.Find(com.EntityId);
                if (comm == null)
                {
                    return BadRequest("No comment with that id");
                }
                var top = db.Topics.Find(comm.TopicId);
                if(top == null)
                {
                    return BadRequest("no topic with id that is comments parent");
                }

                DeleteAllChildren(comm);

               // db.Comments.Remove(comm);
                db.SaveChanges();
                messageForAuthor = "We have received complaint(s) about your comment: \'" + comm.Text + "\' \n on topic: \'" + top.Name + "\' and after careful consideration we have decided to delete it.";
                messageForSnitches = "We have received your complaint about comment: \'" + comm.Text + "\' \n on topic: \'" + top.Name + "\' and after careful consideration we have decided to delete it.";

                SendMessage(baUser.UserName, comm.AuthorUsername, messageForAuthor);
            }

            var complaints = db.Complaints.Where(c => c.EntityId == complaint.EntityId && c.EntityType == complaint.EntityType && !c.Regulated);
            List<string> snitches = GetSnitches(complaints);
            

            snitches.ForEach(s =>
            {
                SendMessage(baUser.UserName, s, messageForSnitches);
            });

            foreach(var item in complaints)
            {
                item.Regulated = true;
                db.Entry(item).State = EntityState.Modified;
            }
            db.SaveChanges();


            return StatusCode(HttpStatusCode.NoContent);
        }

        private void DeleteVotes(Subforum s)
        {
            var topics = s.Topics.ToList();

            foreach(var t in topics)
            {
                if(t != null)
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


        [Authorize(Roles = "Moderator, Admin")]
        [HttpPut]
        [Route("api/Complaints/{id}/WarnAuthor")]
        [ResponseType(typeof(void))]
        public IHttpActionResult WarnAuthor(int id, Complaint complaint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != complaint.Id)
            {
                return BadRequest();
            }

            var com = db.Complaints.Find(id);
            if (com == null)
            {
                return BadRequest("no complaint with that id");
            }

            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return BadRequest("Not logged in");
            }
            var userStore = new UserStore<BAIdentityUser>(db);
            var userManager = new UserManager<BAIdentityUser>(userStore);

            if (!userManager.IsInRole(baUser.Id, "Admin"))
            {
                List<Complaint> inp = new List<Complaint>();
                inp.Add(complaint);
                List<Complaint> ret = FilterOutNotMine(inp, baUser.UserName);

                if (ret == null || ret.Count == 0)
                {
                    return Unauthorized();
                }
            }

            string messageForAuthor = "";
            string messageForSnitches = "";

            if (com.EntityType == EntityType.Subforum)
            {
                var subf = db.Subforums.Find(com.EntityId);
                if (subf == null)
                {
                    return BadRequest("No subforum with that id");
                }
                
                messageForAuthor = "We have received complaint(s) about your subforum: \'" + subf.Name + "\' and after careful consideration we have decided to only warn you.";
                messageForSnitches = "We have recieved your complaint about subforum: \'" + subf.Name + "\' and after careful consideration we have decided to warn the lead moderator.";

                SendMessage(baUser.UserName, subf.LeadModeratorUsername, messageForAuthor);
            }
            else if (com.EntityType == EntityType.Topic)
            {
                var top = db.Topics.Find(com.EntityId);
                if (top == null)
                {
                    return BadRequest("No topic with that id");
                }
                
                messageForAuthor = "We have received complaint(s) about your topic: \'" + top.Name + "\' and after careful consideration we have decided to only warn you.";
                messageForSnitches = "We have received your complaint about topic: \'" + top.Name + "\' and after careful consideration we have decided to warn the author.";

                SendMessage(baUser.UserName, top.AuthorUsername, messageForAuthor);
            }
            else if (com.EntityType == EntityType.Comment)
            {
                var comm = db.Comments.Find(com.EntityId);
                if (comm == null)
                {
                    return BadRequest("No comment with that id");
                }
                var top = db.Topics.Find(comm.TopicId);
                if (top == null)
                {
                    return BadRequest("no topic with id that is comments parent");
                }
            
                messageForAuthor = "We have received complaint(s) about your comment: \'" + comm.Text + "\' \n on topic: \'" + top.Name + "\' and after careful consideration we have decided to only warn you.";
                messageForSnitches = "We have received your complaint about comment: \'" + comm.Text + "\' \n on topic: \'" + top.Name + "\' and after careful consideration we have decided to warn the author.";

                SendMessage(baUser.UserName, comm.AuthorUsername, messageForAuthor);
            }

            var complaints = db.Complaints.Where(c => c.EntityId == complaint.EntityId && c.EntityType == complaint.EntityType && !c.Regulated);
            List<string> snitches = GetSnitches(complaints);


            snitches.ForEach(s =>
            {
                SendMessage(baUser.UserName, s, messageForSnitches);
            });

            foreach (var item in complaints)
            {
                item.Regulated = true;
                db.Entry(item).State = EntityState.Modified;
            }
            db.SaveChanges();


            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpPut]
        [Route("api/Complaints/{id}/Dismiss")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Dismiss(int id, Complaint complaint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != complaint.Id)
            {
                return BadRequest();
            }

            var com = db.Complaints.Find(id);
            if (com == null)
            {
                return BadRequest("no complaint with that id");
            }

            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return BadRequest("Not logged in");
            }

            var userStore = new UserStore<BAIdentityUser>(db);
            var userManager = new UserManager<BAIdentityUser>(userStore);

            if (!userManager.IsInRole(baUser.Id, "Admin"))
            {
                List<Complaint> inp = new List<Complaint>();
                inp.Add(complaint);
                List<Complaint> ret = FilterOutNotMine(inp, baUser.UserName);

                if (ret == null || ret.Count == 0)
                {
                    return Unauthorized();
                }
            }

            string messageForSnitches = "";

            if (com.EntityType == EntityType.Subforum)
            {
                var subf = db.Subforums.Find(com.EntityId);
                if (subf == null)
                {
                    return BadRequest("No subforum with that id");
                }

                messageForSnitches = "We have recieved your complaint about subforum: \'" + subf.Name + "\' and after careful consideration we have decided to dismiss the complaint.";
                
            }
            else if (com.EntityType == EntityType.Topic)
            {
                var top = db.Topics.Find(com.EntityId);
                if (top == null)
                {
                    return BadRequest("No topic with that id");
                }
                
                messageForSnitches = "We have received your complaint about topic: \'" + top.Name + "\' and after careful consideration we have decided to dismiss the complaint.";
                
            }
            else if (com.EntityType == EntityType.Comment)
            {
                var comm = db.Comments.Find(com.EntityId);
                if (comm == null)
                {
                    return BadRequest("No comment with that id");
                }
                var top = db.Topics.Find(comm.TopicId);
                if (top == null)
                {
                    return BadRequest("no topic with id that is comments parent");
                }
                
                messageForSnitches = "We have received your complaint about comment: \'" + comm.Text + "\' \n on topic: \'" + top.Name + "\' and after careful consideration we have decided to dismiss the complaint.";
                
            }

            var complaints = db.Complaints.Where(c => c.EntityId == complaint.EntityId && c.EntityType == complaint.EntityType && !c.Regulated);
            List<string> snitches = GetSnitches(complaints);


            snitches.ForEach(s =>
            {
                SendMessage(baUser.UserName, s, messageForSnitches);
            });

            foreach (var item in complaints)
            {
                item.Regulated = true;
                db.Entry(item).State = EntityState.Modified;
            }
            db.SaveChanges();


            return StatusCode(HttpStatusCode.NoContent);
        }

        private void DeleteAllChildren(Comment rootComment)
        {
            if (rootComment == null)
            {
                return;
            }

            IQueryable<Comment> cc = db.Comments.Where(c => c.ParentCommentId == rootComment.Id);
            var childrenComments = cc.ToList();

            if (childrenComments == null || childrenComments.Count == 0)
            {
                var votes = db.Votes.Where(v => v.EntityType == EntityType.Comment && v.EntityId == rootComment.Id);
                db.Votes.RemoveRange(votes);

                db.Comments.Remove(rootComment);
                return;
            }

            foreach (var comm in childrenComments)
            {
                DeleteAllChildren(comm);
            }

            var votes2 = db.Votes.Where(v => v.EntityType == EntityType.Comment && v.EntityId == rootComment.Id);
            db.Votes.RemoveRange(votes2);


            db.Comments.Remove(rootComment);
        }


        [Authorize(Roles = "Moderator")]
        [Route("api/Complaints/GetRegulated")]
        [ResponseType(typeof(IQueryable<Complaint>))]
        [HttpGet]
        public IQueryable<Complaint> GetRegulated()
        {
            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return null;
            }


            var userStore = new UserStore<BAIdentityUser>(db);
            var userManager = new UserManager<BAIdentityUser>(userStore);


            if (userManager.IsInRole(baUser.Id, "Admin"))
            {
                return db.Complaints.Where(c => c.Regulated);
            }
            else if (userManager.IsInRole(baUser.Id, "Moderator"))
            {
                var myComps = FilterOutNotMine(db.Complaints.Where(c => c.EntityType != EntityType.Subforum).ToList(), baUser.UserName);

                return myComps.Where(m => m.Regulated).AsQueryable<Complaint>();
            }
            else
            {
                return null;
            }

        }

        [Authorize(Roles = "Moderator")]
        [Route("api/Complaints/GetUnregulated")]
        [ResponseType(typeof(IQueryable<Complaint>))]
        [HttpGet]
        public IQueryable<Complaint> GetUnregulated()
        {
            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return null;
            }


            var userStore = new UserStore<BAIdentityUser>(db);
            var userManager = new UserManager<BAIdentityUser>(userStore);


            if (userManager.IsInRole(baUser.Id, "Admin"))
            {
                return db.Complaints.Where(c => !c.Regulated);
            }
            else if (userManager.IsInRole(baUser.Id, "Moderator"))
            {
                var myComps = FilterOutNotMine(db.Complaints.Where(c => c.EntityType != EntityType.Subforum).ToList(), baUser.UserName);

                return myComps.Where(m => !m.Regulated).AsQueryable<Complaint>();
            }
            else
            {
                return null;
            }

        }

        [Authorize(Roles = "Moderator")]
        [Route("api/Complaints/CountRegulated")]
        [ResponseType(typeof(int))]
        [HttpGet]
        public IHttpActionResult CountRegulated()
        {
            var baUser = db.Users.Find(User.Identity.GetUserName());
            if (baUser == null)
            {
                return BadRequest("not logged in");
            }


            var userStore = new UserStore<BAIdentityUser>(db);
            var userManager = new UserManager<BAIdentityUser>(userStore);


            if (userManager.IsInRole(baUser.Id, "Admin"))
            {
                return Ok(db.Complaints.Count(m => m.Regulated));
            }
            else if (userManager.IsInRole(baUser.Id, "Moderator"))
            {
                var myComps = FilterOutNotMine(db.Complaints.Where(c => c.EntityType != EntityType.Subforum).ToList(), baUser.UserName);

                return Ok(myComps.Count(m => m.Regulated));
            }
            else
            {
                return Unauthorized();
            }

        }

        [Authorize(Roles = "Moderator, Admin")]
        [Route("api/Complaints/CountUnregulated")]
        [ResponseType(typeof(int))]
        [HttpGet]
        public IHttpActionResult CountUnregulated()
        {
            var baUser = db.Users.Find(User.Identity.GetUserName());
            if(baUser == null)
            {
                return BadRequest("not logged in");
            }


            var userStore = new UserStore<BAIdentityUser>(db);
            var userManager = new UserManager<BAIdentityUser>(userStore);


            if (userManager.IsInRole(baUser.Id, "Admin"))
            {
                return Ok(db.Complaints.Count(m => !m.Regulated));
            }
            else if(userManager.IsInRole(baUser.Id, "Moderator"))
            {
                var myComps = FilterOutNotMine(db.Complaints.Where(c => c.EntityType != EntityType.Subforum).ToList(), baUser.UserName);

                return Ok(myComps.Count(m => !m.Regulated));
            }
            else
            {
                return Unauthorized();
            }

        }

        List<Complaint> FilterOutNotMine(List<Complaint> inputList, string username)
        {

            List<Complaint> myComps = new List<Complaint>();

            foreach (var comp in inputList)
            {
                Topic top = null;
                if (comp.EntityType == EntityType.Topic)
                {
                    top = db.Topics.Find(comp.EntityId);
                }
                else if (comp.EntityType == EntityType.Comment)
                {
                    var comm = db.Comments.Find(comp.EntityId);
                    if (comm != null)
                    {
                        top = db.Topics.Find(comm.TopicId);
                    }
                }

                if (top != null)
                {
                    var subf = db.Subforums.Find(top.SubforumId);
                    if (subf != null && subf.LeadModeratorUsername == username)
                    {
                        myComps.Add(comp);
                    }
                }
            }

            return myComps;
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ComplaintExists(int id)
        {
            return db.Complaints.Count(e => e.Id == id) > 0;
        }
    }
}