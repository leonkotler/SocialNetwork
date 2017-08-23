using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using static SocialNetwork.Utils.Utils;

namespace SocialNetwork.DAL
{
    public class DataInitializer : DropCreateDatabaseAlways<NetworkContext>
    {
        protected override void Seed(NetworkContext context)
        {
            User user1 = new User { FirstName = "Eliahu", LastName = "Khalastchi",Password="123", Gender = MyGender.Male, BirthDate = DateTime.Parse("2017-08-09"), Email = "eli@colman.co.il" };
            User user2 = new User { FirstName = "Oren", LastName = "Kapach",Password="123", Gender = MyGender.Male, BirthDate = DateTime.Parse("2015-08-09"), Email = "oren@colman.com" };
            User user3 = new User { FirstName = "Nezer", LastName = "Zaidenberg", Password = "123", Gender = MyGender.Female, BirthDate = DateTime.Parse("2016-08-09"), Email = "nezer@colman.com" };
            User user4 = new User { FirstName = "Galit", LastName = "Haim", Password = "123", Gender = MyGender.Female, BirthDate = DateTime.Parse("2014-08-09"), Email = "galit@colman.com" };

            var users = new List<User>();

            users.Add(user1);
            users.Add(user2);
            users.Add(user3);
            users.Add(user4);

            Post post1 = new Post { PostID = 1, UserID = user1.UserID, Content = "Post 1 content", Likes = 5, PostDate = DateTime.Parse("2011-08-09"), Title = "Post 1 title" };
            Post post2 = new Post { PostID = 2, UserID = user2.UserID, Content = "Post 2 content", Likes = 6, PostDate = DateTime.Parse("2012-08-09"), Title = "Post 2 title" };
            Post post3 = new Post { PostID = 3, UserID = user3.UserID, Content = "Post 3 content", Likes = 7, PostDate = DateTime.Parse("2013-08-09"), Title = "Post 3 title" };
            Post post4 = new Post { PostID = 4, UserID = user4.UserID, Content = "Post 4 content", Likes = 8, PostDate = DateTime.Parse("2014-08-09"), Title = "Post 4 title" };

            var posts = new List<Post>();
            posts.Add(post1);
            posts.Add(post2);
            posts.Add(post3);
            posts.Add(post4);

            List<Post> posts1 = new List<Post>();
            List<Post> posts2 = new List<Post>();
            List<Post> posts3 = new List<Post>();
            List<Post> posts4 = new List<Post>();

            posts1.Add(post1);
            posts2.Add(post2);
            posts3.Add(post3);
            posts4.Add(post4);

            var comments = new List<Comment>
                {
                    new Comment {CommentID =1, UserID=user1.UserID, Content="Comment 1 contents", PostID=1, Title="Comment 1 title",Likes=1 },
                    new Comment {CommentID =2, UserID=user2.UserID, Content="Comment 2 contents", PostID=2, Title="Comment 2 title",Likes=2 },
                    new Comment {CommentID =3, UserID=user3.UserID, Content="Comment 3 contents", PostID=3, Title="Comment 3 title",Likes=3 },
                    new Comment {CommentID =4, UserID=user4.UserID, Content="Comment 4 contents", PostID=4, Title="Comment 4 title",Likes=4 }
                };

            var groups = new List<Group>
                {
                    new Group {GroupID = 1, UserID = user1.UserID, Title="Group 1 title", Likes=600, CreatedDate=DateTime.Parse("2014-08-09"), Posts=posts1, Members=users },
                    new Group {GroupID = 2, UserID = user2.UserID, Title="Group 2 title", Likes=100, CreatedDate=DateTime.Parse("2014-08-09"), Posts=posts2, Members=users  },
                    new Group {GroupID = 3, UserID = user3.UserID, Title="Group 3 title", Likes=2, CreatedDate=DateTime.Parse("2014-08-09"),  Posts=posts3, Members=users  },
                    new Group {GroupID = 4, UserID = user4.UserID, Title="Group 4 title", Likes=1, CreatedDate=DateTime.Parse("2014-08-09"),  Posts=posts4, Members=users  }
                };

            users.ForEach(u => context.Users.Add(u));
            posts.ForEach(p => context.Posts.Add(p));
            comments.ForEach(c => context.Comments.Add(c));
            groups.ForEach(g => context.Groups.Add(g));


            context.SaveChanges();
        }
    }

}