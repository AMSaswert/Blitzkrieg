using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    [Serializable]
    public class Complaint
    {
        public int Id { get; set; }

        [Required]
        [StringLength(2048)]
        public string Text { get; set; }

        public DateTime CreationDate { get; set; }

        [Required]
        public EntityType EntityType { get; set; }

        [Required]
        public int EntityId { get; set; }

        public string EntityName { get; set; }

        [Required]
        public string AuthorUsername { get; set; }

        public bool Regulated { get; set; }
    }
}