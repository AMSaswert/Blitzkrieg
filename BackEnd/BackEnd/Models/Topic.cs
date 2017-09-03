using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    [Serializable]
    public class Topic
    {
        public int Id { get; set; }

        [Required]
        public int SubforumId { get; set; }

        public Subforum Subforum { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        public TopicType TopicType { get; set; }


        [Required]
        public string AuthorUsername { get; set; }

        [Required]
        [StringLength(2048)]
        public string Content { get; set; }

        public DateTime CreationDate { get; set; }

        public int LikesNo { get; set; }

        public int DislikesNo { get; set; }
    }
}