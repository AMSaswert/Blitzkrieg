using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{

    public class AppUser
    {

        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string ContactPhone { get; set; }

        public DateTime RegistrationDate { get; set; }
        
        public List<int> BookmarkedSubforums { get; set; }

        public List<int> SavedTopics { get; set; }

        public List<int> SavedComments { get; set; }

        public List<Message> ReceivedMessages { get; set; }




    }
}