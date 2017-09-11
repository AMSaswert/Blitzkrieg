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
            Subforum sub = Models.Models.Subforums.Where(x => x.Topics.Where(y => y.Id == id).FirstOrDefault() == x.Topics.Where(y => y.Id == id).FirstOrDefault()).FirstOrDefault();

            return sub.Topics.Where(x => x.Id == id).FirstOrDefault();
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
            sub.Topics.Remove(sub.Topics.Where(x => x.Id == top.Id).FirstOrDefault());
            sub.Topics.Add(top);
            serializer.SerializeObject(Models.Models.Subforums, "Subforums");
        }

        // DELETE: api/Topics/5
        public void Delete(int id)
        {
            Subforum sub = Models.Models.Subforums.Where(x => x.Topics.Where(y => y.Id == id).FirstOrDefault() == x.Topics.Where(y => y.Id == id).FirstOrDefault()).FirstOrDefault();
            sub.Topics.Remove(sub.Topics.Where(x => x.Id == id).FirstOrDefault());
            serializer.SerializeObject(Models.Models.Subforums, "Subforums");
        }
    }
}
