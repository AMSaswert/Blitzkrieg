using BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEnd.App_Start
{
    public class ModelsConfig
    {
        static DataIO serializer = new DataIO();
        static string address = @"E:\Fax\Web 1\Web1 - slave\BackEnd\TestData\bin\Debug\";
        public static void Deserialze()
        {
            Models.Models.AppUsers.AddRange(serializer.DeSerializeObject<List<AppUser>>(address + "AppUsers"));
            Models.Models.CustomBAIdentityUsers.AddRange(serializer.DeSerializeObject<List<CustomBAIdentityUser>>(address + "CustomBAIdentityUsers"));
            Models.Models.Comments.AddRange(serializer.DeSerializeObject<List<Comment>>(address + "Comments"));
            Models.Models.Complaints.AddRange(serializer.DeSerializeObject<List<Complaint>>(address + "Complaints"));
            Models.Models.Messages.AddRange(serializer.DeSerializeObject<List<Message>>(address + "Messages"));
            Models.Models.Subforums.AddRange(serializer.DeSerializeObject<List<Subforum>>(address + "Subforums"));
            Models.Models.SubforumModerations.AddRange(serializer.DeSerializeObject<List<SubforumModeration>>(address + "SubforumModerations"));
            Models.Models.Topics.AddRange(serializer.DeSerializeObject<List<Topic>>(address + "Topics"));
            Models.Models.Votes.AddRange(serializer.DeSerializeObject<List<Vote>>(address + "Votes"));
        }
    }
}