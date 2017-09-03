using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    [Serializable]
    public class AppUser
    {

        public AppUser()
        {
            this.BookmarkedSubforums = new List<Subforum>();
            this.SavedComments = new List<Comment>();
            this.SavedTopics = new List<Topic>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string Surname { get; set; }

        [StringLength(256)]
        public string ContactPhone { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        // regularni korisnik
        public List<Subforum> BookmarkedSubforums { get; set; }
        public List<Topic> SavedTopics { get; set; }
        public List<Comment> SavedComments { get; set; }




    }
}