﻿using BackEnd.Models;
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
        public List<Complaint> Get(string username)
        {
            List<Complaint> complaints = new List<Complaint>();

            List<string> liableUsers = new List<string>();

            foreach (var complaint in Models.Models.Complaints)
            {
                foreach (var subforum in Models.Models.Subforums)
                {
                    if (subforum.Id == complaint.EntityId)
                    {
                        if(LieabilityCheck(username,subforum.LeadModeratorUsername))
                        {
                            complaints.Add(complaint);
                            break;
                        }
                    }

                    foreach (var topic in subforum.Topics)
                    {
                        if (topic.Id == complaint.EntityId)
                        {
                            if (LieabilityCheck(username, subforum.LeadModeratorUsername))
                            {
                                complaints.Add(complaint);
                                break;
                            }
                        }

                        foreach (var item in topic.Comments)
                        {
                            if (item.Id == complaint.EntityId)
                            {
                                if (LieabilityCheck(username, subforum.LeadModeratorUsername))
                                {
                                    complaints.Add(complaint);
                                    break;
                                }
                            }
                            else
                            {
                                bool condition = EditComment(item.ChildrenComments, complaint.EntityId);
                                if (condition)
                                {
                                    if (LieabilityCheck(username, subforum.LeadModeratorUsername))
                                    {
                                        complaints.Add(complaint);
                                        break;
                                    }
                                }
                            }
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

        private bool EditComment(List<Comment> comments, int id)
        {
            foreach (var item in comments)
            {
                if (item.Id == id)
                {
                    return true;
                }
                else
                {
                    EditComment(item.ChildrenComments, id);
                    break;
                }
            }
            return false;
        }

        private bool LieabilityCheck(string username,string leadModerator)
        {
            foreach (var user in Models.Models.AppUsers)
            {
                if ((user.Role == "Admin" && user.UserName == username) || leadModerator == username)
                {
                    return true;
                    
                }
            }
            return false;
        }
    }
}
