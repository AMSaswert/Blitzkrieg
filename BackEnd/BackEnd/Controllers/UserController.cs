using BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackEnd.Controllers
{
    public class UserController : ApiController
    {
        public AppUser Get(string username)
        {
            return Models.Models.AppUsers.Where(x => x.UserName == username).FirstOrDefault();
        }
    }
}
