using BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackEnd.Controllers
{
    public class DislikesAndLikesController : ApiController
    {

        // GET: api/DislikesAndLikes/5
        public int[] Get(string username)
        {
            int[] dislikesAndLikes = new int[2];
            dislikesAndLikes[0] = 0;
            dislikesAndLikes[1] = 0;

            foreach (var subforum in Models.Models.Subforums)
            {
                foreach (var topic in subforum.Topics)
                {
                    if(topic.AuthorUsername == username)
                    {
                        dislikesAndLikes[1] += topic.LikesNum;
                        dislikesAndLikes[0] += topic.DislikesNum;
                    }
                    foreach (var item in topic.Comments)
                    {
                        if (item.AuthorUsername == username)
                        {
                            dislikesAndLikes[1] += item.LikesNo;
                            dislikesAndLikes[0] += item.DislikesNo;
                        }
                        else
                        {
                            ChildrenComments(item.ChildrenComments, ref dislikesAndLikes, username);
                        }
                    }
                }
            }
            return dislikesAndLikes;
        }


        private void ChildrenComments(List<Comment> comments, ref int[] dislikesAndLikes, string username)
        {
            foreach (var item in comments)
            {
                if (item.AuthorUsername == username)
                {
                    dislikesAndLikes[1] += item.LikesNo;
                    dislikesAndLikes[0] += item.DislikesNo;
                }
                else
                {
                    ChildrenComments(item.ChildrenComments, ref dislikesAndLikes, username);
                }
            }
        }
    }
}
    
