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

namespace BackEnd.Controllers
{
    public class MessagesController : ApiController
    {
        private BAContext db = new BAContext();

        // GET: api/Messages
       /* public IQueryable<Message> GetMessages()
        {
            return db.Messages;
        }*/
        
        // GET: api/Messages/5
        [ResponseType(typeof(Message))]
        public IHttpActionResult GetMessage(int id)
        {
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }
        /*
        // PUT: api/Messages/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMessage(int id, Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != message.Id)
            {
                return BadRequest();
            }

            if (!db.Users.Any(u => u.UserName == message.SenderUsername))
            {
                return BadRequest("No user with sender's username");
            }

            if (!db.Users.Any(u => u.UserName == message.RecipientUsername))
            {
                return BadRequest("No user with recipient's username");
            }

            db.Entry(message).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
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

        // POST: api/Messages
        [ResponseType(typeof(Message))]
        public IHttpActionResult PostMessage(Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!db.Users.Any(u => u.UserName == message.SenderUsername))
            {
                return BadRequest("No user with sender's username");
            }

            if (!db.Users.Any(u => u.UserName == message.RecipientUsername))
            {
                return BadRequest("No user with recipient's username");
            }
            message.CreationDate = DateTime.Now;

            db.Messages.Add(message);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = message.Id }, message);
        }
        */
        // DELETE: api/Messages/5
        /*[ResponseType(typeof(Message))]
        public IHttpActionResult DeleteMessage(int id)
        {
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            db.Messages.Remove(message);
            db.SaveChanges();

            return Ok(message);
        }*/

        [Authorize(Roles ="AppUser, Moderator, Admin")]
        [Route("api/Messages/MarkAsSeen")]
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult MarkAsSeen(Message message)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!db.Messages.Any(mess => mess.Id == message.Id))
            {
                return BadRequest("No message with that Id");
            }
            
            var m = db.Messages.Find(message.Id);

            if(m.RecipientUsername != User.Identity.GetUserName())
            {
                return BadRequest("Can not mark as seen if you are not recipient");
            }

            if(m.Seen)
            {
                return BadRequest("Message already marked as seen");
            }

            m.Seen = true;

            db.Entry(m).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }catch
            {
                return InternalServerError();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [Route("api/Messages/DeletePermanently")]
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult DeletePermanently(Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!db.Messages.Any(mess => mess.Id == message.Id))
            {
                return BadRequest("No message with that Id");
            }

            var m = db.Messages.Find(message.Id);

            if (m.RecipientUsername != User.Identity.GetUserName() && m.SenderUsername != User.Identity.GetUserName())
            {
                return BadRequest("Can not modify someone elses messages");
            }

            if ((m.RecipientUsername == User.Identity.GetUserName() && !m.RecipientDeleted) ||
                (m.SenderUsername == User.Identity.GetUserName() && !m.SenderDeleted))
            {
                return BadRequest("Message not in trash");
            }

            if ((m.RecipientUsername == User.Identity.GetUserName() && m.RecipientDeleted && m.RecipientDeletedForReal) ||
                (m.SenderUsername == User.Identity.GetUserName() && m.SenderDeleted && m.SenderDeletedForReal))
            {
                return BadRequest("Message already deleted permanently");
            }

            if(m.SenderUsername == m.RecipientUsername)
            {
                db.Messages.Remove(m);
                db.SaveChanges();
            }
            else if((m.RecipientDeletedForReal && m.SenderUsername == User.Identity.GetUserName()) || 
                    (m.SenderDeletedForReal && m.RecipientUsername == User.Identity.GetUserName()))
            {
                db.Messages.Remove(m);
                db.SaveChanges();
            }
            else
            {
                if (m.RecipientUsername == User.Identity.GetUserName())
                {
                    m.RecipientDeletedForReal = true;
                }
                else if (m.SenderUsername == User.Identity.GetUserName())
                {
                    m.SenderDeletedForReal = true;
                }

                db.Entry(m).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    return InternalServerError();
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }


        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [Route("api/Messages/RestoreFromTrash")]
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult RestoreFromTrash(Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!db.Messages.Any(mess => mess.Id == message.Id))
            {
                return BadRequest("No message with that Id");
            }

            var m = db.Messages.Find(message.Id);

            if (m.RecipientUsername != User.Identity.GetUserName() && m.SenderUsername != User.Identity.GetUserName())
            {
                return BadRequest("Can not modify someone elses messages");
            }

            if ((m.RecipientUsername == User.Identity.GetUserName() && !m.RecipientDeleted) ||
                (m.SenderUsername == User.Identity.GetUserName() && !m.SenderDeleted))
            {
                return BadRequest("Message not in trash");
            }

            if ((m.RecipientUsername == User.Identity.GetUserName() && m.RecipientDeleted && m.RecipientDeletedForReal) ||
                (m.SenderUsername == User.Identity.GetUserName() && m.SenderDeleted && m.SenderDeletedForReal))
            {
                return BadRequest("Message was deleted permanently");
            }

            if(m.RecipientUsername == m.SenderUsername)
            {
                m.RecipientDeleted = false;
                m.SenderDeleted = false;
            }
            else if (m.RecipientUsername == User.Identity.GetUserName())
            {
                m.RecipientDeleted = false;
            }
            else if (m.SenderUsername == User.Identity.GetUserName())
            {
                m.SenderDeleted = false;
            }

            db.Entry(m).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch
            {
                return InternalServerError();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [Route("api/Messages/MoveToTrash")]
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult MoveToTrash(Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!db.Messages.Any(mess => mess.Id == message.Id))
            {
                return BadRequest("No message with that Id");
            }

            var m = db.Messages.Find(message.Id);

            if (m.RecipientUsername != User.Identity.GetUserName() && m.SenderUsername != User.Identity.GetUserName())
            {
                return BadRequest("Can not modify someone elses messages");
            }

            if ((m.RecipientUsername == User.Identity.GetUserName() && m.RecipientDeleted) || 
                (m.SenderUsername == User.Identity.GetUserName() && m.SenderDeleted))
            {
                return BadRequest("Message already in trash");
            }

            if(m.RecipientUsername == m.SenderUsername)
            {
                m.RecipientDeleted = true;
                m.SenderDeleted = true;
            }
            else if(m.RecipientUsername == User.Identity.GetUserName())
            {
                m.RecipientDeleted = true;
            }
            else if(m.SenderUsername == User.Identity.GetUserName())
            {
                m.SenderDeleted = true;
            }

            db.Entry(m).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch
            {
                return InternalServerError();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }



        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [Route("api/Messages/InboxHasUnread")]
        [ResponseType(typeof(int))]
        [HttpGet]
        public IHttpActionResult InboxHasUnread()
        {
            var username = User.Identity.GetUserName();
            return Ok(db.Messages.Any(m => m.RecipientUsername == username && !m.Seen && !m.RecipientDeletedForReal && !m.RecipientDeleted));
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [Route("api/Messages/TrashHasUnread")]
        [ResponseType(typeof(int))]
        [HttpGet]
        public IHttpActionResult TrashHasUnread()
        {
            var username = User.Identity.GetUserName();
            return Ok(db.Messages.Any(m => m.RecipientUsername == username && !m.Seen && !m.RecipientDeletedForReal && m.RecipientDeleted));
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [Route("api/Messages/CountUnread")]
        [ResponseType(typeof(int))]
        [HttpGet]
        public IHttpActionResult CountUnread()
        {
            var username = User.Identity.GetUserName();
            return Ok(db.Messages.Count(m => m.RecipientUsername == username && !m.Seen && !m.RecipientDeletedForReal && ! m.RecipientDeleted));
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [Route("api/Messages/CountInbox")]
        [ResponseType(typeof(int))]
        [HttpGet]
        public IHttpActionResult CountInbox()
        {
            var username = User.Identity.GetUserName();
            return Ok(db.Messages.Count(m => m.RecipientUsername == username && !m.RecipientDeleted && !m.RecipientDeletedForReal));
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [Route("api/Messages/CountSent")]
        [ResponseType(typeof(int))]
        [HttpGet]
        public IHttpActionResult CountSent()
        {
            var username = User.Identity.GetUserName();
            return Ok(db.Messages.Count(m => m.SenderUsername == username && !m.SenderDeleted && ! m.SenderDeletedForReal));
        }

        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [Route("api/Messages/CountTrash")]
        [ResponseType(typeof(int))]
        [HttpGet]
        public IHttpActionResult CountTrash()
        {
            var username = User.Identity.GetUserName();
            return Ok(db.Messages.Count(m => (m.RecipientUsername == username && m.RecipientDeleted && !m.RecipientDeletedForReal) ||
                                            (m.SenderUsername == username && m.SenderDeleted && !m.SenderDeletedForReal)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageExists(int id)
        {
            return db.Messages.Count(e => e.Id == id) > 0;
        }
    }
}