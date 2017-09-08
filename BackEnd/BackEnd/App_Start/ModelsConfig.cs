using BackEnd.Controllers;
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
        static string address = @"C:\Users\Saswert\Desktop\Blitzkrieg\BackEnd\TestData\bin\Debug\";
        public static void Deserialze()
        {
            Models.Models.AppUsers.AddRange(serializer.DeSerializeObject<List<AppUser>>(address + "AppUsers"));
            Models.Models.Complaints.AddRange(serializer.DeSerializeObject<List<Complaint>>(address + "Complaints"));
            Models.Models.Subforums.AddRange(serializer.DeSerializeObject<List<Subforum>>(address + "Subforums"));
        }
    }
}