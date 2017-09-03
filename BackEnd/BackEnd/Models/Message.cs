using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    [Serializable]
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string SenderUsername { get; set; }

        [Required]
        public string RecipientUsername { get; set; }

        [Required]
        [StringLength(256)]
        public string Subject { get; set; }

        [Required]
        [StringLength(4096)]
        public string Content { get; set; }

        public DateTime CreationDate { get; set; }

        public bool Seen { get; set; }

        public bool RecipientDeleted { get; set; }
        public bool SenderDeleted { get; set; }


        public bool RecipientDeletedForReal { get; set; }
        public bool SenderDeletedForReal { get; set; }
    }
}