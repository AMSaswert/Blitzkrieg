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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Subforums/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Subforums
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Subforums/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Subforums/5
        public void Delete(int id)
        {
        }
    }
}
