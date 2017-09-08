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
        string address = @"C:\Users\Saswert\Desktop\Blitzkrieg\BackEnd\TestData\bin\Debug\";
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
            Subforum sub = Models.Models.Subforums.Where(x => x.Topics.Where(y => y.Id == id).FirstOrDefault() == x.Topics.Where(y => y.Id == id).FirstOrDefault()).FirstOrDefault();
            Topic topic = sub.Topics.Where(x => x.Id == id).FirstOrDefault();
            topic.Comments.Add(comm);
            serializer.SerializeObject(Models.Models.Subforums,address + "Subforums");
        }

        // DELETE: api/Comments/5

        public void Delete(int id,int commentId)
        {
            Subforum sub = Models.Models.Subforums.Where(x => x.Topics.Where(y => y.Id == id).FirstOrDefault() == x.Topics.Where(y => y.Id == id).FirstOrDefault()).FirstOrDefault();
            Topic topic = sub.Topics.Where(x => x.Id == id).FirstOrDefault();
            Comment comment = topic.Comments.Where(x => x.Id == commentId).FirstOrDefault();
            List<Comment> list = comment.init(comment.ChildrenComments);
            comment.ChildrenComments.Clear();
            comment.ChildrenComments.AddRange(list);
            serializer.SerializeObject(Models.Models.Subforums, address + "Subforums");

        }
    }
}
