using BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData
{
    class Program
    {
        static List<AppUser> AppUsers = new List<AppUser>();
        static List<Complaint> Complaints = new List<Complaint>();
        static List<Subforum> Subforums = new List<Subforum>();
        static void Main(string[] args)
        {
            string address = @"E:\Fax\Web 1\Web1 - slave\BackEnd\BackEnd\bin\";
            DataIO serializer = new DataIO();
            Random random = new Random((int)DateTime.Now.Ticks);
            AppUsers.Add(new AppUser()
            {
                Id = random.Next(),
                Name = "admin",
                Surname = "admin",
                UserName = "admin",
                ContactPhone = "021/12345",
                Email = "admin@yahoo.com",
                Password = "admin",
                Role = "Admin",
                RegistrationDate = DateTime.Now
            });
            AppUsers.Add(new AppUser()
            {
                Id = random.Next(),
                Name = "Aleksandar",
                Surname = "Misljenovic",
                UserName = "aca",
                Email = "aca@yahoo.com",
                Password = "aca",
                Role = "AppUser",
                ContactPhone = "021/1353545",
                RegistrationDate = DateTime.Now
            });

            AppUsers.Add(new AppUser()
            {
                Id = random.Next(),
                Name = "Moderator",
                Surname = "Moderator",
                ContactPhone = "021/8778585",
                UserName = "moderator",
                Email = "moderator@yahoo.com",
                Password = "moderator",
                Role = "Moderator",
                RegistrationDate = DateTime.Now
            });

            CreateSubforum(random, "Funny", "hot topics", "do w/e", "moderator", "http://localhost:54042/Content/Icons/icon-0.ico");
            CreateSubforum(random, "World", "controversial topics", "w/e", "appu", "http://localhost:54042/Content/Icons/icon-1.ico");
            CreateSubforum(random, "Movies", "new topics", "w/e", "moderator", "http://localhost:54042/Content/Icons/icon-2.ico");
            CreateSubforum(random, "Gaming", "opular topics", "w/e", "appu", "http://localhost:54042/Content/Icons/icon-3.ico");
            Subforum sub = Subforums[0];
            CreateTopic(random, "Topic 1 bla bla bla bla bla bla bla bla bla bla blaaaaaaa",
                        "babinskizoltan", TopicType.Text, "test test", Subforums[0]);
            CreateTopic(random, "Topic 2 bla bla", "babinskizoltan", TopicType.Text, "test test", Subforums[1]);
            CreateTopic(random, "Topic 3 bla bla bla bla bla bla bla bla ", "babinskizoltan", TopicType.Text, "test test", Subforums[2]);
            CreateTopic(random, "Topic 4 bla bla bla bla bla", "babinskizoltan", TopicType.Text, "test test", Subforums[3]);
            CreateTopic(random, "Topic 5 ", "babinskizoltan", TopicType.Text, "test test", Subforums[0]);
            CreateTopic(random, "Topic 8 bla bla bla bla bla bla bla bla bla bla bla", "babinskizoltan", TopicType.Text, "test test", Subforums[0]);
            CreateTopic(random, "Topic 7 bla bla bla bla bla bla bla bla bla bla bla", "babinskizoltan", TopicType.Text, "test test", Subforums[1]);
            CreateTopic(random, "Topic 8 bla ", "babinskizoltan", TopicType.Text, "test test", Subforums[2]);
            CreateTopic(random, "Topic 6 bla bla bla bla ", "babinskizoltan", TopicType.Text, "test test", Subforums[3]);

            CreateComment(random, "is gud", "aca", Subforums[0], 0);
            CreateComment(random, "is gsdadasdasasddsa", "aca", Subforums[1], 1);
            CreateComment(random, "is gadssdasadsdaasdud", "aca", Subforums[2], 1);
            CreateComment(random, "is adsdasasddsaasdasdsdadassaddasdadagud", "aca", Subforums[3], 1);
            CreateComment(random, "asddsadsdsais gadsadsasddsaud", "aca", Subforums[0], 1);
            CreateComment(random, "is ssadasdasdadsasddsaasddsasdagud", "aca", Subforums[1], 1);
            CreateComment(random, "idasdasdsas gud", "aca", Subforums[2], 1);
            CreateComment(random, "is dasdsasdaadsgud", "aca", Subforums[3], 1);
            CreateComment(random, "isdasdasdsaads gud", "aca", Subforums[0], 0);
            CreateChildComment(random, "ojsaaaaaa", "aca", Subforums[0], 0,Subforums[0].Topics[0].Comments[0].Id);

            serializer.SerializeObject(AppUsers, "AppUsers");
            serializer.SerializeObject(Complaints, "Complaints");
            serializer.SerializeObject(Subforums, "Subforums");
            

        }

        static void CreateSubforum(Random random, string name, string description, string rules, string leadModerator, string iconURL)
        {
            Subforums.Add(new Subforum()
            {
                Id = random.Next(),
                Name = name,
                Description = description,
                Rules = rules,
                LeadModeratorUsername = leadModerator,
                IconURL = iconURL,
                Topics = new List<Topic>()
            });
        }

        static void CreateTopic(Random random, string name, string authorUsername, TopicType topicType, string content, Subforum sub)
        {
            sub.Topics.Add(new Topic()
            {
                Id = random.Next(),

                Name = name,
                AuthorUsername = authorUsername,
                TopicType = topicType,
                Content = content,
                CreationDate = DateTime.Now,
                LikesNum = 0,
                DislikesNum = 0,
                Comments = new List<Comment>()
            });
        }

        static void CreateComment(Random random, string text, string authorUsername, Subforum sub, int index)
        {
            //var topic = Topics.Where(t => true).FirstOrDefault();
            //var commCount = Comments.Count(comm => true);
            //Comment c;
            //if (commCount > 0)
            //{
            //    c = Comments.Where(comm2 => true).FirstOrDefault();
            //}
            //else
            //{
            //    c = null;
            //}
            //int? pcommId = c?.Id;
            sub.Topics[index].Comments.Add(new Comment()
            {
                Id = random.Next(),
                Text = text,
                TopicId = sub.Topics[index].Id,
                CreationDate = DateTime.Now,
                AuthorUsername = authorUsername,
                ChildrenComments = new List<Comment>()
            });
        }

        static void CreateChildComment(Random random, string text, string authorUsername, Subforum sub, int index,int parentCommentId)
        {
            sub.Topics[index].Comments[0].ChildrenComments.Add(new Comment()
            {
                Id = random.Next(),
                Text = text,
                TopicId = sub.Topics[index].Id,
                CreationDate = DateTime.Now,
                AuthorUsername = authorUsername,
                ParentCommentId = parentCommentId
            });
        }
    }
}
