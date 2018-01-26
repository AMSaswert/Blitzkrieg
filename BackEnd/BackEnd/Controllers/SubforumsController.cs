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
    public class SubforumsController : ApiController
    {

        // GET: api/Subforums
        DataIO serializer = new DataIO();
        public IQueryable<Subforum> Get()
        {
            return Models.Models.Subforums.AsQueryable();
        }

        // GET: api/Subforums/5
        public Subforum Get(int id)
        {
            return Models.Models.Subforums.Where(x => x.Id == id).FirstOrDefault();
        }

        // POST: api/Subforums
        public void Post(object subforum)
        {
            Subforum sub = JsonConvert.DeserializeObject<Subforum>(subforum.ToString());
            Models.Models.Subforums.Add(sub);
            serializer.SerializeObject(Models.Models.Subforums, "Subforums");
        }

        // PUT: api/Subforums/5
        public void Put(int id, object subforum)
        {
            Subforum sub = JsonConvert.DeserializeObject<Subforum>(subforum.ToString());
            if(Models.Models.Subforums.Where(x => x.Id == sub.Id).FirstOrDefault() != null)
            {
                Models.Models.Subforums[Models.Models.Subforums.FindIndex(x => x.Id == id)] = sub;
            }
            else
            {
                Models.Models.Subforums.Add(sub);
            }
            serializer.SerializeObject(Models.Models.Subforums, "Subforums");
        }

        // DELETE: api/Subforums/5
        [HttpDelete]
        public void Delete(int id)
        {
            Models.Models.Subforums.Remove(Models.Models.Subforums.Where(x => x.Id == id).FirstOrDefault());
            serializer.SerializeObject(Models.Models.Subforums, "Subforums");
        }
    }
}
