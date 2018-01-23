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
        // GET: api/Complaints
        public List<Complaint> Get()
        {
            return Models.Models.Complaints;
        }

        // GET: api/Complaints/5
        public List<Complaint> Get(string useRole)
        {
            string[] userAndRole = useRole.Split('-');
            List<Complaint> complaints = new List<Complaint>();
            bool outOfLoop = false;

            foreach (var complaint in Models.Models.Complaints)
            {

                if (complaint.EntityType == EntityType.Subforum)
                {
                    if (LieabilityCheck(userAndRole[0], userAndRole[1], string.Empty))
                    {
                        complaints.Add(complaint);
                    }
                }
                else if (complaint.EntityType == EntityType.Topic)
                {
                    Subforum subforum = Models.Models.Subforums.Where(x => x.Topics.Where(y => y.Id == complaint.EntityId).FirstOrDefault() ==
                    x.Topics.Where(y => y.Id == complaint.EntityId).FirstOrDefault()).FirstOrDefault();
                    if (LieabilityCheck(userAndRole[0], userAndRole[1], subforum.LeadModeratorUsername))
                    {
                        complaints.Add(complaint);
                    }
                }
                else
                {
                    foreach (var subforum in Models.Models.Subforums)
                    {
                        foreach (var topic in subforum.Topics)
                        {
                            foreach (var item in topic.Comments)
                            {
                                if (item.Id == complaint.EntityId)
                                {
                                    if (LieabilityCheck(userAndRole[0], userAndRole[1], subforum.LeadModeratorUsername))
                                    {
                                        complaints.Add(complaint);
                                        outOfLoop = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (FindComment(item.ChildrenComments, complaint.EntityId))
                                    {
                                        if (LieabilityCheck(userAndRole[0], userAndRole[1], subforum.LeadModeratorUsername))
                                        {
                                            complaints.Add(complaint);
                                            outOfLoop = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (outOfLoop == true)
                            {
                                break;
                            }
                        }
                        if (outOfLoop == true)
                        {
                            outOfLoop = false;
                            break;
                        }
                    }
                }
            }
            return complaints;
        }

        // POST: api/Complaints
        public void Post(object complaint)
        {
            Complaint compl = JsonConvert.DeserializeObject<Complaint>(complaint.ToString());
            Models.Models.Complaints.Add(compl);
            serializer.SerializeObject(Models.Models.Complaints,"Complaints");

        }

        // PUT: api/Complaints/5
        public void Put(int id, object complaint)
        {
            Complaint compl = JsonConvert.DeserializeObject<Complaint>(complaint.ToString());
            Models.Models.Complaints.Remove(Models.Models.Complaints.Where(x => x.Id == id).FirstOrDefault());
            Models.Models.Complaints.Add(compl);
            serializer.SerializeObject(Models.Models.Complaints,"Complaints");
        }

        // DELETE: api/Complaints/5
        [HttpDelete]
        public void Delete(int id)
        {
            Models.Models.Complaints.Remove(Models.Models.Complaints.Where(x => x.Id == id).FirstOrDefault());
            serializer.SerializeObject(Models.Models.Complaints,"Complaints");
        }

        private bool FindComment(List<Comment> comments, int id)
        {
            foreach (var item in comments)
            {
                if (item.Id == id)
                {
                    return true;
                }
                else
                {
                    if(FindComment(item.ChildrenComments, id))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool LieabilityCheck(string username, string role, string leadModerator)
        {
            if (role == "Admin")
            {
                return true;
            }
            else if (username == leadModerator)
            {
                return true;
            }
            return false;
        }
    }
}
