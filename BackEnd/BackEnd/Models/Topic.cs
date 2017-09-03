using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    public enum TopicType
    {
        Text,
        Picture,
        Link
    }

    [Serializable]
    public class Topic
    {
        public int Id { get; set; }

        public int SubforumId { get; set; }

        public string Name { get; set; }

        public TopicType TopicType { get; set; }

        public string AuthorUsername { get; set; }

        public string Content { get; set; }

        public DateTime CreationDate { get; set; }

        public int LikesNum { get; set; }

        public int DislikesNum { get; set; }

        public List<Comment> Comments { get; set; }

        public List<AppUser> UsersWhoVoted { get; set; }
    }
}