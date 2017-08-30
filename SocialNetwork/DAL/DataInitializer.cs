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
            string eliAvatar = "https://www.colman.ac.il/sites/default/files/styles/165x202/public/eli_small.jpg";
            string orenAvatar = "http://he.switchbee.com/wp-content/uploads/2016/07/image-5.png";
            string igorAvatar = "https://www.colman.ac.il/sites/default/files/styles/165x202/public/igor_small.jpg?";
            string nezerAvatar = "https://media.licdn.com/mpr/mpr/shrinknp_200_200/p/2/005/03d/277/01fedf9.jpg";


            User user1 = new User { UserID=1, IsAdmin = true, FirstName = "Eliahu", LastName = "Khalastchi",Password="123", Gender = MyGender.Male, BirthDate = DateTime.Parse("2017-08-09"), Email = "eli@colman.co.il" , ImageUrl=eliAvatar};
            User user2 = new User { UserID=2, FirstName = "Oren", LastName = "Kapach",Password="123", Gender = MyGender.Male, BirthDate = DateTime.Parse("2015-08-09"), Email = "oren@colman.co.il", ImageUrl = orenAvatar };
            User user3 = new User { UserID=3, FirstName = "Nezer", LastName = "Zaidenberg", Password = "123", Gender = MyGender.Male, BirthDate = DateTime.Parse("2016-08-09"), Email = "nezer@colman.co.il", ImageUrl = nezerAvatar };
            User user4 = new User { UserID=4, IsAdmin=true ,FirstName = "Igor", LastName = "Rochlin", Password = "123", Gender = MyGender.Male, BirthDate = DateTime.Parse("2014-08-09"), Email = "igor@colman.co.il", ImageUrl = igorAvatar };

            var users = new List<User>();

            users.Add(user1);
            users.Add(user2);
            users.Add(user3);
            users.Add(user4);

            Post post1 = new Post { PostID = 1, GroupId = 1, User = user1, Content = "Post 1 content", Likes = 5, PostDate = DateTime.Parse("2011-08-09"), Title = "Post 1 title" };
            Post post2 = new Post { PostID = 2, GroupId = 2, User = user2, Content = "Post 2 content", Likes = 6, PostDate = DateTime.Parse("2012-08-09"), Title = "Post 2 title" };
            Post post3 = new Post { PostID = 3, User = user3, Content = "Post 3 content", Likes = 7, PostDate = DateTime.Parse("2013-08-09"), Title = "Post 3 title" };
            Post post4 = new Post { PostID = 4, User = user4, Content = "Post 4 content", Likes = 8, PostDate = DateTime.Parse("2014-08-09"), Title = "Post 4 title" };
            Post post5 = new Post { PostID = 5, User = user4, Content = "Post 5 content", Likes = 8, PostDate = DateTime.Parse("2014-08-09"), Title = "Post 5 title" };
            Post post6 = new Post { PostID = 6, User = user4, Content = "Post 6 content", Likes = 8, PostDate = DateTime.Parse("2014-08-09"), Title = "Post 6 title" };
            Post post7 = new Post { PostID = 7, User = user4, Content = "Post 7 content", Likes = 8, PostDate = DateTime.Parse("2014-08-09"), Title = "Post 7 title" };


            var posts = new List<Post>();
            posts.Add(post1);
            posts.Add(post2);
            posts.Add(post3);
            posts.Add(post4);
            posts.Add(post5);
            posts.Add(post6);
            posts.Add(post7);

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
                    new Comment {CommentID =1, User = user1, Content="Comment 1 contents", Post=post1, Likes=1 },
                    new Comment {CommentID =5, User = user1, Content="Comment 1 contentz", Post=post1, Likes=9 },
                    new Comment {CommentID =2, User = user2, Content="Comment 2 contents", Post=post2, Likes=2 },
                    new Comment {CommentID =3, User = user3, Content="Comment 3 contents", Post=post3, Likes=3 },
                    new Comment {CommentID =4, User = user4, Content="Comment 4 contents", Post=post4, Likes=4 }
                };

            var groups = new List<Group>
                {
                    new Group {GroupID = 1, AdminId = user1.UserID, Title="Group 1 title", Likes=600, CreatedDate=DateTime.Parse("2014-08-09"), Posts=posts1, Members=users },
                    new Group {GroupID = 2, AdminId = user2.UserID, Title="Group 2 title", Likes=100, CreatedDate=DateTime.Parse("2014-08-09"), Posts=posts2, Members=users  },
                    new Group {GroupID = 3, AdminId = user3.UserID, Title="Group 3 title", Likes=2, CreatedDate=DateTime.Parse("2014-08-09"),  Posts=posts3, Members=users  },
                    new Group {GroupID = 4, AdminId = user4.UserID, Title="Group 4 title", Likes=1, CreatedDate=DateTime.Parse("2014-08-09"),  Posts=posts4, Members=users  }
                };

            users.ForEach(u => context.Users.Add(u));
            posts.ForEach(p => context.Posts.Add(p));
            comments.ForEach(c => context.Comments.Add(c));
            groups.ForEach(g => context.Groups.Add(g));


            context.SaveChanges();
        }
    }

}