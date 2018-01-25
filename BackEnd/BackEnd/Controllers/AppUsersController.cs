using BackEnd.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace BackEnd.Controllers
{


   
    public class AppUsersController : ApiController
    {
        DataIO serializer = new DataIO();
        // GET: api/AppUsers
        public IEnumerable<AppUser> Get()
        {
            return Models.Models.AppUsers.AsQueryable();
        }

        // GET: api/AppUsers/5
        public AppUser Login(string usernameAndPassword)
        {
            string[] username_password = usernameAndPassword.Split('-');
            AppUser user = Models.Models.AppUsers.Where(x => x.UserName == username_password[0] && x.Password == username_password[1]).FirstOrDefault() as AppUser;
            if (user != null)
                return user;
            return null;

        }

        // POST: api/AppUsers
        [HttpPost]
        public IHttpActionResult Post(object user)
        {
            AppUser appuser = JsonConvert.DeserializeObject<AppUser>(user.ToString());
            AppUser temp = Models.Models.AppUsers.Where(x => x.UserName == appuser.UserName).FirstOrDefault();
            if (temp != null)
            {
                return BadRequest("Username already exists.");
            }
            else
            {
                Models.Models.AppUsers.Add(appuser);
                serializer.SerializeObject(Models.Models.AppUsers, "AppUsers");
                return Ok("User successfully registered.");
            }
        }

        // PUT: api/AppUsers/5
        [HttpPut]
        public void Put(int id, object user)
        {
            AppUser appuser = JsonConvert.DeserializeObject<AppUser>(user.ToString());
            if(Models.Models.AppUsers.Where(x => x.Id == id).FirstOrDefault() != null)
            {
                Models.Models.AppUsers[Models.Models.AppUsers.FindIndex(x => x.Id == id)] = appuser;
            }
            else
            {
                Models.Models.AppUsers.Add(appuser);
            }
            serializer.SerializeObject(Models.Models.AppUsers,"AppUsers");
        }

        // DELETE: api/AppUsers/5
        public void Delete(int id)
        {
            Models.Models.AppUsers.Remove(Models.Models.AppUsers.Where(x => x.Id == id).FirstOrDefault());
            serializer.SerializeObject(Models.Models.AppUsers,"AppUsers");
        }
    }
}
