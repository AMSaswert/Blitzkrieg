using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    [Serializable]
    public class Comment
    {
        public int Id { get; set; }

        public int TopicId { get; set; }

        public string AuthorUsername { get; set; }

        public DateTime CreationDate { get; set; }

        public int? ParentCommentId { get; set; }

        public List<Comment> ChildrenComments { get; set; }

        public string Text { get; set; }

        public int LikesNo { get; set; }

        public int DislikesNo { get; set; }

        public bool Edited { get; set; }

        public bool Removed { get; set; }

        public List<AppUser> UsersWhoVoted { get; set; }
        
    }
}