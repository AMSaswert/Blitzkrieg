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
        static List<CustomBAIdentityUser> CustomBAIdentityUsers = new List<CustomBAIdentityUser>();
        static List<Comment> Comments = new List<Comment>();
        static List<Complaint> Complaints = new List<Complaint>();
        static List<Message> Messages = new List<Message>();
        static List<Subforum> Subforums = new List<Subforum>();
        static List<SubforumModeration> SubforumModerations = new List<SubforumModeration>();
        static List<Topic> Topics = new List<Topic>();
        static List<Vote> Votes = new List<Vote>();
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
                ContactPhone = "021/12345",
                RegistrationDate = DateTime.Now
            });
            AppUsers.Add(new AppUser()
            {
                Id = random.Next(),
                Name = "Aleksandar",
                Surname = "Misljenovic",
                ContactPhone = "021/1353545",
                RegistrationDate = DateTime.Now
            });

            AppUsers.Add(new AppUser()
            {
                Id = random.Next(),
                Name = "Moderator",
                Surname = "Moderator",
                ContactPhone = "021/8778585",
                RegistrationDate = DateTime.Now
            });

            var _appUser = AppUsers.FirstOrDefault(a => a.Name == "admin" && a.Surname == "admin");
            CustomBAIdentityUsers.Add(new CustomBAIdentityUser() { Id = "admin", UserName = "admin", Email = "admin@yahoo.com",Password = "admin",Role="Admin" , AppUserId = _appUser.Id });
            _appUser = AppUsers.FirstOrDefault(a => a.Name == "Aleksandar" && a.Surname == "Misljenovic");
            CustomBAIdentityUsers.Add(new CustomBAIdentityUser() { Id = "aca", UserName = "aca", Email = "aca@yahoo.com", Password = "aca", Role = "AppUser", AppUserId = _appUser.Id });
            _appUser = AppUsers.FirstOrDefault(a => a.Name == "Moderator" && a.Surname == "Moderator");
            CustomBAIdentityUsers.Add(new CustomBAIdentityUser() { Id = "moderator", UserName = "moderator", Email = "moderator@yahoo.com", Password = "moderator", Role = "Moderator", AppUserId = _appUser.Id });

            CreateSubforum(random, "Funny", "hot topics", "do w/e", "moderator", "http://localhost:54042/Content/Icons/icon-0.ico");
            CreateSubforum(random, "World", "controversial topics", "w/e", "appu", "http://localhost:54042/Content/Icons/icon-1.ico");
            CreateSubforum(random, "Movies", "new topics", "w/e", "moderator", "http://localhost:54042/Content/Icons/icon-2.ico");
            CreateSubforum(random, "Gaming", "opular topics", "w/e", "appu", "http://localhost:54042/Content/Icons/icon-3.ico");


            CreateSubforumModerations(random, "aca", "Funny");
            CreateSubforumModerations(random, "moderator", "World");
            CreateSubforumModerations(random, "aca", "Movies");
            CreateSubforumModerations(random, "moderator", "Gaming");

            CreateTopic(random, "Topic 1 bla bla bla bla bla bla bla bla bla bla blaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                        "babinskizoltan", TopicType.Text, "test test", "Funny");
            CreateTopic(random, "Topic 2 bla bla", "babinskizoltan", TopicType.Text, "test test", "Funny");
            CreateTopic(random, "Topic 3 bla bla bla bla bla bla bla bla ", "babinskizoltan", TopicType.Text, "test test", "World");
            CreateTopic(random, "Topic 4 bla bla bla bla bla", "babinskizoltan", TopicType.Text, "test test", "World");
            CreateTopic(random, "Topic 5 ", "babinskizoltan", TopicType.Text, "test test", "Funny");
            CreateTopic(random, "Topic 8 bla bla bla bla bla bla bla bla bla bla bla", "babinskizoltan", TopicType.Text, "test test", "Funny");
            CreateTopic(random, "Topic 7 bla bla bla bla bla bla bla bla bla bla bla", "babinskizoltan", TopicType.Text, "test test", "Funny");
            CreateTopic(random, "Topic 8 bla ", "babinskizoltan", TopicType.Text, "test test", "World");
            CreateTopic(random, "Topic 6 bla bla bla bla ", "babinskizoltan", TopicType.Text, "test test", "World");

            CreateComment(random, "is gud", "aca");
            CreateComment(random, "is gsdadasdasasddsa", "aca");
            CreateComment(random, "is gadssdasadsdaasdud", "aca");
            CreateComment(random, "is adsdasasddsaasdasdsdadassaddasdadagud", "aca");
            CreateComment(random, "asddsadsdsais gadsadsasddsaud", "aca");
            CreateComment(random, "is ssadasdasdadsasddsaasddsasdagud", "aca");
            CreateComment(random, "idasdasdsas gud", "aca");
            CreateComment(random, "is dasdsasdaadsgud", "aca");
            CreateComment(random, "isdasdasdsaads gud", "aca");

            serializer.SerializeObject(AppUsers,"AppUsers");
            serializer.SerializeObject(CustomBAIdentityUsers,"CustomBAIdentityUsers");
            serializer.SerializeObject(Comments,"Comments");
            serializer.SerializeObject(Complaints,"Complaints");
            serializer.SerializeObject(Messages,"Messages");
            serializer.SerializeObject(Subforums,"Subforums");
            serializer.SerializeObject(SubforumModerations,"SubforumModerations");
            serializer.SerializeObject(Topics,"Topics");
            serializer.SerializeObject(Votes,"Votes");
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
                IconURL = iconURL
            });
        }

        static void CreateSubforumModerations(Random random, string moderatorUser, string subforumName)
        {
            var subf = Subforums.Where(s => s.Name == subforumName).FirstOrDefault();

            if (subf == null)
            {
                return;
            }
            SubforumModerations.Add(new SubforumModeration()
            {
                Id = random.Next(),
                ModeratorUsername = moderatorUser,
                SubforumId = subf.Id
            });
        }

        static void CreateTopic(Random random, string name, string authorUsername, TopicType topicType, string content, string subforumName)
        {
            var subf = Subforums.Where(s => s.Name == subforumName).FirstOrDefault();
            if (subf == null)
            {
                return;
            }

            Topics.Add(new Topic()
            {
                Id = random.Next(),
                Name = name,
                AuthorUsername = authorUsername,
                TopicType = topicType,
                Content = content,
                CreationDate = DateTime.Now,
                LikesNo = 0,
                DislikesNo = 0,
                SubforumId = subf.Id
            });
        }

        static void CreateComment(Random random, string text, string authorUsername)
        {
            var topic = Topics.Where(t => true).FirstOrDefault();
            var commCount = Comments.Count(comm => true);
            Comment c;
            if (commCount > 0)
            {
                c = Comments.Where(comm2 => true).FirstOrDefault();
            }
            else
            {
                c = null;
            }
            int? pcommId = c?.Id;
            Comments.Add(new Comment()
            {
                Id = random.Next(),
                Text = text,
                TopicId = topic.Id,
                ParentCommentId = pcommId,
                CreationDate = DateTime.Now,
                AuthorUsername = authorUsername
            });
        }
    }
}
