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
    public class ComplaintsController : ApiController
    {
        DataIO serializer = new DataIO();
        string address = @"C:\Users\Saswert\Desktop\Blitzkrieg\BackEnd\TestData\bin\Debug\";
        // GET: api/Complaints
        public IQueryable<Complaint> Get()
        {
            return Models.Models.Complaints.AsQueryable();
        }

        // GET: api/Complaints/5
        public Complaint Get(int id)
        {
            return Models.Models.Complaints.Where(x => x.Id == id).FirstOrDefault();
        }

        // POST: api/Complaints
        public void Post(object complaint)
        {
            Complaint compl = JsonConvert.DeserializeObject<Complaint>(complaint.ToString());
            Models.Models.Complaints.Add(compl);
            serializer.SerializeObject(Models.Models.Complaints,address + "Complaints");

        }

        // PUT: api/Complaints/5
        public void Put(int id, object complaint)
        {
            Complaint compl = JsonConvert.DeserializeObject<Complaint>(complaint.ToString());
            Models.Models.Complaints.Remove(Models.Models.Complaints.Where(x => x.Id == id).FirstOrDefault());
            Models.Models.Complaints.Add(compl);
            serializer.SerializeObject(Models.Models.Complaints, address + "Complaints");
        }

        // DELETE: api/Complaints/5
        [HttpDelete]
        public void Delete(int id)
        {
            Models.Models.Complaints.Remove(Models.Models.Complaints.Where(x => x.Id == id).FirstOrDefault());
            serializer.SerializeObject(Models.Models.Complaints, address + "Complaints");
        }
    }
}
