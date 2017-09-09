using BackEnd.Controllers;
using BackEnd.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BackEnd.App_Start
{
    public class ModelsConfig
    {
        static DataIO serializer = new DataIO();
        public static void Deserialze()
        {
            Models.Models.AppUsers.AddRange(serializer.DeSerializeObject<List<AppUser>>("AppUsers"));
            Models.Models.Complaints.AddRange(serializer.DeSerializeObject<List<Complaint>>("Complaints"));
            Models.Models.Subforums.AddRange(serializer.DeSerializeObject<List<Subforum>>("Subforums"));
        }
    }
}