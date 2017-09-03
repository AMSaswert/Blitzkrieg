using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    public class Models
    {      
        public static List<AppUser> AppUsers = new List<AppUser>();
        public static List<CustomBAIdentityUser> CustomBAIdentityUsers = new List<CustomBAIdentityUser>();
        public static List<Comment> Comments = new List<Comment>();
        public static List<Complaint> Complaints = new List<Complaint>();
        public static List<Message> Messages = new List<Message>();
        public static List<Subforum> Subforums = new List<Subforum>();
        public static List<SubforumModeration> SubforumModerations = new List<SubforumModeration>();
        public static List<Topic> Topics = new List<Topic>();
        public static List<Vote> Votes = new List<Vote>();
        
    }
}