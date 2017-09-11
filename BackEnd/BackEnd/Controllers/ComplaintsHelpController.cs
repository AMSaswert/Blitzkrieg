using BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackEnd.Controllers
{
    public class ComplaintsHelpController : ApiController
    {
        // GET: api/ComplaintsHelp
        //public IEnumerable<string> Get()
        //{
            
        //}

        // GET: api/ComplaintsHelp/5
        public string Get(int id)
        {

            string modUserName = "";
            foreach (var item in Models.Models.Subforums)
            {
                if (item.Topics.Where(x => x.Id == id).FirstOrDefault() != null)
                {
                    modUserName = item.LeadModeratorUsername;
                    break;
                }
            }
            return modUserName;
        }

        //// POST: api/ComplaintsHelp
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/ComplaintsHelp/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ComplaintsHelp/5
        //public void Delete(int id)
        //{
        //}
    }
}
