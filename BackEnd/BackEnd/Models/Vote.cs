using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    [Serializable]
    public class Vote
    {
        public int Id { get; set; }

        [Required]
        public string AuthorUsername { get; set; }

        [Required]
        public EntityType EntityType { get; set; }
        [Required]
        public int EntityId { get; set; }

        [Required]
        public bool Liked { get; set; }
    }
}