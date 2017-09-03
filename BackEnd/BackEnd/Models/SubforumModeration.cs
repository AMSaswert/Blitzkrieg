using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    [Serializable]
    public class SubforumModeration
    {
        public int Id { get; set; }

        [Required]
        public string ModeratorUsername { get; set; }

        [Required]
        public int SubforumId { get; set; }
        public Subforum Subforum { get; set; }
    }
}