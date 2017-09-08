using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    
    public class Message
    {
        public int Id { get; set; }

        public string SenderUsername { get; set; }

        public string RecipientUsername { get; set; }

        public string Content { get; set; }

        public bool Seen { get; set; }
    }
}