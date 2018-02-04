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
                RegistrationDate = DateTime.Now,
                BookmarkedSubforums = new List<string>(),
                ReceivedMessages = new List<Message>() { new Message { Id = random.Next(), SenderUsername = "aca", Content = "Bem ti lebac" }, new Message { Id = random.Next(), SenderUsername = "aca", Content = "aaaaaaaaa" } },
                SavedComments = new List<Comment>(),
                SavedTopics = new List<Topic>()
                
            });
            AppUsers.Add(new AppUser()
            {
                Id = random.Next(),
                Name = "mica",
                Surname = "micic",
                UserName = "mica",
                ContactPhone = "021/12345",
                Email = "mica@yahoo.com",
                Password = "mica",
                Role = "Moderator",
                RegistrationDate = DateTime.Now,
                BookmarkedSubforums = new List<string>(),
                ReceivedMessages = new List<Message>() { new Message { Id = random.Next(),SenderUsername = "aca", Content = "Bem ti lebac" }, new Message { Id = random.Next(), SenderUsername = "aca", Content = "aaaaaaaaa" } },
                SavedComments = new List<Comment>(),
                SavedTopics = new List<Topic>()

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
                RegistrationDate = DateTime.Now,
                BookmarkedSubforums = new List<string>(),
                ReceivedMessages = new List<Message>() { new Message { Id = random.Next(), SenderUsername = "aca", Content = "opassada" }, new Message { Id = random.Next(), SenderUsername = "aca", Content = "ajsasa" } },
                SavedComments = new List<Comment>(),
                SavedTopics = new List<Topic>()
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
                RegistrationDate = DateTime.Now,
                BookmarkedSubforums = new List<string>(),
                ReceivedMessages = new List<Message>() { new Message { Id = random.Next(), SenderUsername = "aca", Content = "mhmhm" }, new Message { Id = random.Next(), SenderUsername = "aca", Content = "frik" } },
                SavedComments = new List<Comment>(),
                SavedTopics = new List<Topic>()
            });

            AppUsers.Add(new AppUser()
            {
                Id = random.Next(),
                Name = "Aleksandar",
                Surname = "Misljenovic",
                UserName = "borba",
                Email = "borba@yahoo.com",
                Password = "borba",
                Role = "AppUser",
                ContactPhone = "021/1353545",
                RegistrationDate = DateTime.Now,
                BookmarkedSubforums = new List<string>(),
                ReceivedMessages = new List<Message>() { new Message { Id = random.Next(), SenderUsername = "aca", Content = "opassada" }, new Message { Id = random.Next(), SenderUsername = "aca", Content = "ajsasa" } },
                SavedComments = new List<Comment>(),
                SavedTopics = new List<Topic>()
            });

            AppUsers.Add(new AppUser()
            {
                Id = random.Next(),
                Name = "Aleksandar",
                Surname = "Misljenovic",
                UserName = "kec",
                Email = "kec@yahoo.com",
                Password = "kec",
                Role = "AppUser",
                ContactPhone = "021/1353545",
                RegistrationDate = DateTime.Now,
                BookmarkedSubforums = new List<string>(),
                ReceivedMessages = new List<Message>() { new Message { Id = random.Next(), SenderUsername = "aca", Content = "opassada" }, new Message { Id = random.Next(), SenderUsername = "aca", Content = "ajsasa" } },
                SavedComments = new List<Comment>(),
                SavedTopics = new List<Topic>()
            });

            CreateSubforum(random, "Funny", "hot topics", "do w/e", "moderator", "http://localhost:54042/Content/Icons/icon-0.ico");
            CreateSubforum(random, "World", "controversial topics", "w/e", "mica", "http://localhost:54042/Content/Icons/icon-1.ico");
            CreateSubforum(random, "Movies", "new topics", "w/e", "mica", "http://localhost:54042/Content/Icons/icon-2.ico");
            CreateSubforum(random, "Gaming", "opular topics", "w/e", "moderator", "http://localhost:54042/Content/Icons/icon-3.ico");
            Subforum sub = Subforums[0];
            CreateTopic(random, "Topic 1",
                        "mica", TopicType.Text, "test test", Subforums[0]);
            CreateTopic(random, "Topic 2", "mica", TopicType.Text, "test test", Subforums[1]);
            CreateTopic(random, "Topic 3", "mica", TopicType.Text, "test test", Subforums[2]);
            CreateTopic(random, "Topic 4", "mica", TopicType.Text, "test test", Subforums[3]);
            CreateTopic(random, "Topic 5 ", "mica", TopicType.Text, "test test", Subforums[0]);
            CreateTopic(random, "Topic 8", "mica", TopicType.Text, "test test", Subforums[0]);
            CreateTopic(random, "Topic 7", "mica", TopicType.Text, "test test", Subforums[1]);
            CreateTopic(random, "Topic 8", "mica", TopicType.Text, "test test", Subforums[2]);
            CreateTopic(random, "Topic 6", "mica", TopicType.Text, "test test", Subforums[3]);

            CreateComment(random, "jedan", "mica", Subforums[0], 0);
            CreateComment(random, "dva", "mica", Subforums[1], 1);
            CreateComment(random, "tri", "mica", Subforums[2], 1);
            CreateComment(random, "cetiri", "mica", Subforums[3], 1);
            CreateComment(random, "pet", "aca", Subforums[0], 1);
            CreateComment(random, "sest", "aca", Subforums[1], 1);
            CreateComment(random, "sedam", "aca", Subforums[2], 1);
            CreateComment(random, "osam", "aca", Subforums[3], 1);
            CreateComment(random, "devet", "aca", Subforums[0], 0);
            CreateChildComment(random, "deset", "aca", Subforums[0], 0,Subforums[0].Topics[0].Comments[0].Id);
            CreateComplaint(random, "zzzzzzzzzz", EntityType.Comment,Subforums[0].Topics[0].Comments[0].Id, Subforums[0].Topics[0].Comments[0].TopicId, Subforums[0].Topics[0].Comments[0].AuthorUsername, "aca");
            CreateComplaint(random, "bbbbbbbbbb", EntityType.Subforum,Subforums[0].Id,-1,Subforums[0].LeadModeratorUsername, "aca");
            CreateComplaint(random, "kkkkkkkkkk", EntityType.Topic, Subforums[0].Topics[0].Id,-1, Subforums[0].Topics[0].AuthorUsername, "aca");

            serializer.SerializeObject(AppUsers, "AppUsers");
            serializer.SerializeObject(Complaints, "Complaints");
            serializer.SerializeObject(Subforums, "Subforums");
            

        }

        static void CreateComplaint(Random random,string text,EntityType type,int entityId,int topicId,string entityAuthor,string AuthorUsername)
        {
            Complaints.Add(new Complaint()
            {
                Id = random.Next(),
                Text = text,
                EntityType = type,
                EntityId = entityId,
                TopicId = topicId,
                EntityAuthor = entityAuthor,
                AuthorUsername = AuthorUsername,
                CreationDate = DateTime.Now
            });
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
                SubforumId = sub.Id,
                Name = name,
                AuthorUsername = authorUsername,
                TopicType = topicType,
                Content = content,
                CreationDate = DateTime.Now,
                LikesNum = 2,
                DislikesNum = 0,
                Comments = new List<Comment>(),
                UsersWhoVoted = new List<string>() { "borba","kec"}
            });
        }

        static void CreateComment(Random random, string text, string authorUsername, Subforum sub, int index)
        {
            sub.Topics[index].Comments.Add(new Comment()
            {
                Id = random.Next(),
                Text = text,
                TopicId = sub.Topics[index].Id,
                CreationDate = DateTime.Now,
                AuthorUsername = authorUsername,
                ChildrenComments = new List<Comment>(),
                UsersWhoVoted = new List<string>(),
                DislikesNo = 0,
                LikesNo = 0,
                Edited = false,
                Removed = false
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
                ParentCommentId = parentCommentId,
                UsersWhoVoted = new List<string>(),
                ChildrenComments = new List<Comment>(),
                DislikesNo = 0,
                Edited = false,
                LikesNo = 0,
                Removed = false
            });
        }
    }
}
