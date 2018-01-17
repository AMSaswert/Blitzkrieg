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


        // GET: api/ComplaintsHelp/5
        public Complaint Get(int id)
        {
            return Models.Models.Complaints.Where(x => x.Id == id).FirstOrDefault();
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
