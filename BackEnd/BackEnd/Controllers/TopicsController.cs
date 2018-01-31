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
    public class TopicsController : ApiController
    {
        DataIO serializer = new DataIO();
        // GET: api/Topics
        public IQueryable<Topic> Get()
        {
            List<Topic> topics = new List<Topic>();
            foreach (var x in Models.Models.Subforums)
            {
                topics.AddRange(x.Topics);
            }
            return topics.AsQueryable();
        }

        // GET: api/Topics/5
        public Topic Get(int id)
        {
            var topic = from subforum in Models.Models.Subforums
                          from top in subforum.Topics
                          where top.Id == id
                          select top;
              
            return topic.ToList().FirstOrDefault();
        }

        // POST: api/Topics
        //public void Post(object topic)
        //{


        //}

        // PUT: api/Topics/5
        public void Put(int id, object topic)
        {
            Topic top = JsonConvert.DeserializeObject<Topic>(topic.ToString());
            Subforum sub = Models.Models.Subforums.Where(x => x.Id == id).FirstOrDefault();
            if (sub.Topics.Where(x => x.Id == top.Id).FirstOrDefault() != null)
            {
                sub.Topics[sub.Topics.FindIndex(x => x.Id == top.Id)] = top;
            }
            else
            {
                sub.Topics.Add(top);
            }
            serializer.SerializeObject(Models.Models.Subforums, "Subforums");
        }

        // DELETE: api/Topics/5
        public IHttpActionResult Delete(int id)
        {
            var temp = from subforum in Models.Models.Subforums
                       from top in subforum.Topics
                       where top.Id == id
                       select subforum;

            Subforum sub = temp.ToList().FirstOrDefault();
            if(sub.Topics.Where(x => x.Id == id).FirstOrDefault() == null)
            {
                return BadRequest("Topic is already deleted.");
            }
            sub.Topics.Remove(sub.Topics.Where(x => x.Id == id).FirstOrDefault());
            serializer.SerializeObject(Models.Models.Subforums, "Subforums");
            return Ok("Topic is deleted.");
        }
    }
}
