using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
  
    public class Subforum
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string IconURL { get; set; }

        public string Rules { get; set; }

        public string LeadModeratorUsername { get; set; }
        
        public List<String> Moderators { get; set; }

        public List<Topic> Topics { get; set; }

    }
}