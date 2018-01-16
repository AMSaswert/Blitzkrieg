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
        public List<string> Get(int id)
        {

            List<string> liableUsers = new List<string>();
            foreach (var subforum in Models.Models.Subforums)
            {
                if (subforum.Id == id)
                {
                    AddAdmins(liableUsers);
                    return liableUsers;
                }

                foreach (var topic in subforum.Topics)
                {
                    if (topic.Id == id)
                    {
                        AddAdmins(liableUsers);
                        liableUsers.Add(subforum.LeadModeratorUsername);
                        return liableUsers;
                    }

                    foreach (var item in topic.Comments)
                    {
                        if(item.Id == id)
                        {
                            AddAdmins(liableUsers);
                            liableUsers.Add(subforum.LeadModeratorUsername);
                            return liableUsers;
                        }
                        else
                        {
                           bool condition =  EditComment(item.ChildrenComments, id);
                           if(condition)
                            {
                                AddAdmins(liableUsers);
                                liableUsers.Add(subforum.LeadModeratorUsername);
                                return liableUsers;
                            }
                        }
                    }
                }
            }
            return liableUsers;
        }


        public bool EditComment(List<Comment> comments, int id)
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

        private void AddAdmins(List<string> users)
        {
            foreach (var user in Models.Models.AppUsers)
            {
                if (user.Role == "Admin")
                {
                    users.Add(user.UserName);
                }
            }
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
