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


   
    public class AppUsersController : ApiController
    {
        DataIO serializer = new DataIO();
        string address = @"C:\Users\Saswert\Desktop\Blitzkrieg\BackEnd\TestData\bin\Debug\";
        // GET: api/AppUsers
        public IEnumerable<AppUser> Get()
        {
            return Models.Models.AppUsers.AsQueryable();
        }

        // GET: api/AppUsers/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/AppUsers
        [HttpPost]
        public void Post(object user)
        {
            AppUser appuser = JsonConvert.DeserializeObject<AppUser>(user.ToString());
            Models.Models.AppUsers.Add(appuser);
            serializer.SerializeObject(Models.Models.AppUsers, address + "AppUsers");
        }

        // PUT: api/AppUsers/5
        [HttpPut]
        public void Put(int id, object user)
        {
            AppUser appuser = JsonConvert.DeserializeObject<AppUser>(user.ToString());
            Models.Models.AppUsers.Remove(Models.Models.AppUsers.Where(x => x.Id == id).FirstOrDefault());
            Models.Models.AppUsers.Add(appuser);
            serializer.SerializeObject(Models.Models.AppUsers, address + "AppUsers");
        }

        // DELETE: api/AppUsers/5
        public void Delete(int id)
        {
            Models.Models.AppUsers.Remove(Models.Models.AppUsers.Where(x => x.Id == id).FirstOrDefault());
            serializer.SerializeObject(Models.Models.AppUsers, address + "AppUsers");
        }
    }
}
