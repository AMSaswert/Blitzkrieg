using BackEnd.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackEnd.Controllers
{
    public class MessagesController : ApiController
    {

        DataIO serializer = new DataIO();
        // GET: api/Messages
        [HttpGet]
        public IQueryable<Message> Get(string username)
        {
            AppUser au = Models.Models.AppUsers.Where(e => e.UserName == username).FirstOrDefault();
            return au.ReceivedMessages.AsQueryable();
        }

        // GET: api/Messages/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Messages
        //[HttpPost]
        //public void Post(Object message)
        //{
        //    //AppUser au = Models.Models.AppUsers.Where(e => e.Id == id).FirstOrDefault();
        //    //au.ReceivedMessages.Add(message);
        //    //serializer.SerializeObject(Models.Models.AppUsers, address + "AppUsers");

        //}

        //PUT: api/Messages/5
        [HttpPut]
        public IHttpActionResult Put(string username, Object message)
        {
            Message msg = JsonConvert.DeserializeObject<Message>(message.ToString());
            AppUser au = Models.Models.AppUsers.Where(e => e.UserName == username).FirstOrDefault();
            //au.ReceivedMessages.Remove(au.ReceivedMessages.Where(y => y.Id == msg.Id).FirstOrDefault());
            //au.ReceivedMessages.Add(msg);
            Message temp = au.ReceivedMessages.Where(y => y.Id == msg.Id).FirstOrDefault();
            if (temp == null)
            {
                au.ReceivedMessages.Add(msg);
            }
            else
            {
                au.ReceivedMessages[au.ReceivedMessages.IndexOf(temp)] = msg;
            }

            serializer.SerializeObject(Models.Models.AppUsers, "AppUsers");
            return Ok("Message recivied.");
            //Message msg = JsonConvert.DeserializeObject<Message>(message.ToString());
            //AppUser au = Models.Models.AppUsers.Where(e => e.Id == id).FirstOrDefault();
            //foreach(var x in au.ReceivedMessages)
            //{
            //    if(msg.Id == x.Id)
            //    {
            //        au.ReceivedMessages.Remove(x);
            //        au.ReceivedMessages.Add(msg);
            //        serializer.SerializeObject(Models.Models.AppUsers, address + "AppUsers");
            //        return;
            //    }
            //}
            //au.ReceivedMessages.Add(msg);
            //serializer.SerializeObject(Models.Models.AppUsers, address + "AppUsers");
        }

        // DELETE: api/Messages/5
        [HttpDelete]
        public void Delete(int id)
        {
            //Message msg = JsonConvert.DeserializeObject<Message>(message.ToString());
            AppUser au = Models.Models.AppUsers.Where(e => e.ReceivedMessages.Where(y => y.Id == id).FirstOrDefault() == e.ReceivedMessages.Where(y => y.Id == id).FirstOrDefault()).FirstOrDefault();
            au.ReceivedMessages.Remove(au.ReceivedMessages.Where(y => y.Id == id).FirstOrDefault());
            serializer.SerializeObject(Models.Models.AppUsers, "AppUsers");
        }
    }
}
