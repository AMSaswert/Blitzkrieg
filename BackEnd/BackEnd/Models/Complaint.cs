using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    public enum EntityType
    {
        Subforum,
        Topic,
        Comment,
    }

    public class Complaint
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreationDate { get; set; }

        public EntityType EntityType { get; set; }

        public int EntityId { get; set; }

        public int? TopicId { get; set; }

        public string EntityAuthor { get; set; }

        public string AuthorUsername { get; set; }
    }
}