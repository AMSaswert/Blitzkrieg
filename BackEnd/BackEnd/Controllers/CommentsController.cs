using BackEnd.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace BackEnd.Controllers
{

    public class CommentsController : ApiController
    {
        DataIO serializer = new DataIO();
        // GET: api/Comments
        public IQueryable<Comment> Get(int id)
        {
            var temp = from subforum in Models.Models.Subforums
                       from top in subforum.Topics
                       where top.Id == id
                       select top;
            Topic topic = temp.ToList().FirstOrDefault();
            if (topic.Comments != null)
                return topic.Comments.AsQueryable();
            return null;
        }

        // PUT: api/Comments/5
        public void Put(int id, object comment)
        {
            Comment comm = JsonConvert.DeserializeObject<Comment>(comment.ToString());
            var temp = from subforum in Models.Models.Subforums
                        from top in subforum.Topics
                        where top.Id == id
                        select top;
            Topic topic = temp.ToList().FirstOrDefault();
            if (comm.ParentCommentId == null)
            {
                if (topic.Comments.Where(x => x.Id == comm.Id).FirstOrDefault() == null)
                {
                    topic.Comments.Add(comm);
                }
                else
                {
                    topic.Comments[topic.Comments.FindIndex(x => x.Id == comm.Id)] = comm;
                }
            }
            else
            {
                foreach (var item in topic.Comments)
                {
                    if (item.Id == comm.ParentCommentId)
                    {                    
                        if(item.ChildrenComments.Where(x => x.Id == comm.Id).FirstOrDefault() != null)
                        {
                            item.ChildrenComments[item.ChildrenComments.FindIndex(x => x.Id == comm.Id)] = comm;
                        }
                        else
                        {
                            item.ChildrenComments.Add(comm);
                        }
                        break;
                    }
                    else
                    {
                        if(EditComment(item.ChildrenComments,comm))
                        {
                            break;
                        }
                    }
                }
            }
            serializer.SerializeObject(Models.Models.Subforums,"Subforums");
        }

        // DELETE: api/Comments/5

        public IHttpActionResult Delete(string commentAndTopicIds)
        {
            string[] strtemp = commentAndTopicIds.Split('-');
            int commentId = Int32.Parse(strtemp[0]);
            int topicId = Int32.Parse(strtemp[1]);

            var temp = from subforum in Models.Models.Subforums
                       from top in subforum.Topics
                       where top.Id == topicId
                       select top;

            Topic topic = temp.ToList().FirstOrDefault();
            Comment comment = topic.Comments.Where(x => x.Id == commentId).FirstOrDefault();
            if (comment == null)
            {
                foreach (var item in topic.Comments)
                {
                    comment = FindChildComment(item.ChildrenComments, commentId);
                    if (comment != null)
                    {
                        break;
                    }
                }
            }
            if (comment != null)
            {
                if (comment.Removed != true)
                {
                    comment.Removed = true;
                    DeleteComments(comment.ChildrenComments);
                }
                else
                {
                    return BadRequest("Comment is already deleted.");
                }
            }
            serializer.SerializeObject(Models.Models.Subforums,"Subforums");
            return Ok("Comment is deleted.");
        }

        public Comment FindChildComment(List<Comment> comments, int id)
        {
            Comment comment = null;
            foreach (var item in comments)
            {
                if (item.Id == id)
                {

                    return item;
                }
                else
                {
                    comment = FindChildComment(item.ChildrenComments, id);
                    if(comment != null)
                    {
                        return comment;
                    }

                }
            }

            return null;

        }

        public void DeleteComments(List<Comment> comments)
        {
            foreach (var comment in comments)
            {
                comment.Removed = true;
                if (comment.ChildrenComments.Any())
                {
                    DeleteComments(comment.ChildrenComments);
                }
            }
        }

        public bool EditComment(List<Comment> comments, Comment comm)
        {
            foreach (var item in comments)
            {
                if (item.Id == comm.ParentCommentId)
                {
                    
                    if (item.ChildrenComments.Where(x => x.Id == comm.Id).FirstOrDefault() != null)
                    {
                        item.ChildrenComments[item.ChildrenComments.FindIndex(x => x.Id == comm.Id)] = comm;
                    }
                    else
                    {
                        item.ChildrenComments.Add(comm);
                    }
                    return true;
                }
                else
                {
                    if(EditComment(item.ChildrenComments,comm))
                    {
                        return true;
                    }
                    
                }
            }

            return false;          
        }
    }
}
