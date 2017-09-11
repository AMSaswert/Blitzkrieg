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

    public class CommentsController : ApiController
    {
        DataIO serializer = new DataIO();
        // GET: api/Comments
        public IQueryable<Comment> Get(int id)
        {
            Subforum sub = Models.Models.Subforums.Where(x => x.Topics.Where(y => y.Id == id).FirstOrDefault() == x.Topics.Where(y => y.Id == id).FirstOrDefault()).FirstOrDefault();
            Topic topic = sub.Topics.Where(x => x.Id == id).FirstOrDefault();
            return topic.Comments.AsQueryable();
        }

        // GET: api/Comments/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Comments
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Comments/5
        public void Put(int id, object comment)
        {
            Comment comm = JsonConvert.DeserializeObject<Comment>(comment.ToString());
            Subforum sub = Models.Models.Subforums.Where(x => x.Topics.Where(y => y.Id == comm.TopicId).FirstOrDefault() == x.Topics.Where(y => y.Id == comm.TopicId).FirstOrDefault()).FirstOrDefault();
            Topic topic = sub.Topics.Where(x => x.Id == comm.TopicId).FirstOrDefault();
            Comment parentComment = new Comment();
            Comment childComment = new Comment();
            if (comm.ParentCommentId == null)
            {
                if (topic.Comments.Where(x => x.Id == comm.Id).FirstOrDefault() == null)
                {
                    topic.Comments.Add(comm);
                }
                else
                {
                    childComment = topic.Comments.Where(x => x.Id == comm.Id).FirstOrDefault();
                    topic.Comments.Remove(childComment);
                    topic.Comments.Add(comm);
                }
            }
            else
            {
                foreach (var item in topic.Comments)
                {
                    if (item.Id == comm.ParentCommentId)
                    {
                        item.ChildrenComments.Remove(item.ChildrenComments.Where(x => x.Id == comm.Id).FirstOrDefault());
                        item.ChildrenComments.Add(comm);
                        break;
                    }
                    else
                    {
                        EditComment(item.ChildrenComments, comm);
                        break;
                    }
                }
            }
            serializer.SerializeObject(Models.Models.Subforums,"Subforums");
        }

        // DELETE: api/Comments/5

        public void Delete(int id)
        {
            Subforum sub = Models.Models.Subforums.Where(x => x.Topics.Where(y => 
            y.Comments.Where(z => z.Id == id).FirstOrDefault() == y.Comments.Where(z => 
            z.Id == id).FirstOrDefault()).FirstOrDefault() == x.Topics.Where(y => 
            y.Comments.Where(z => z.Id == id).FirstOrDefault() == y.Comments.Where(z => 
            z.Id == id).FirstOrDefault()).FirstOrDefault()).FirstOrDefault();
            Topic topic = sub.Topics.Where(x => x.Comments.Where(y => y.Id == id).FirstOrDefault() == x.Comments.Where(y => y.Id == id).FirstOrDefault()).FirstOrDefault();
            Comment comment = topic.Comments.Where(x => x.Id == id).FirstOrDefault();
            if (comment == null)
            {
                foreach (var item in topic.Comments)
                {
                    comment = FindChildComment(item, id);
                    if (comment != null)
                    {
                        break;
                    }
                }
            }
            comment.Removed = true;
            DeleteComments(comment.ChildrenComments);
            serializer.SerializeObject(Models.Models.Subforums,"Subforums");

        }

        public Comment FindChildComment(Comment comment, int id)
        {
            Comment childComment = comment.ChildrenComments.Where(x => x.Id == id).FirstOrDefault();
            if (childComment == null)
            {
                return FindChildComment(childComment, id);
            }
            else
            {
                return childComment;
            }
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

        public void EditComment(List<Comment> comments, Comment comm)
        {
            foreach (var item in comments)
            {
                if (item.Id == comm.ParentCommentId)
                {
                    item.ChildrenComments.Remove(item.ChildrenComments.Where(x => x.Id == comm.Id).FirstOrDefault());
                    item.ChildrenComments.Add(comm);
                    break;
                }
                else
                {
                    EditComment(item.ChildrenComments, comm);
                    break;
                }
            }
        }
    }
}
