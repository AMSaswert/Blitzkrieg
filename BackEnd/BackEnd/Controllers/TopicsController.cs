using BackEnd.Models;
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
        string address = @"C:\Users\Saswert\Desktop\Blitzkrieg\BackEnd\TestData\bin\Debug\";
        // GET: api/Topics
        public IQueryable<Topic> Get()
        {
            List<Topic> topics = new List<Topic>();
            foreach(var x in Models.Models.Subforums)
            {
                topics.AddRange(x.Topics);
            }
            return topics.AsQueryable();
        }

        // GET: api/Topics/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Topics
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Topics/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Topics/5
        public void Delete(int id)
        {
        }
    }
}
