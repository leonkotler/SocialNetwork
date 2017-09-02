using SocialNetwork.DAL;
using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Controllers
{
    public class AdminController : Controller
    {
        private NetworkContext db = new NetworkContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["Admin"] != null)
                base.OnActionExecuting(filterContext);
            else
                filterContext.Result = RedirectToAction("AccessDenied","Welcome", new { ErrorMessage = "Restricted to administrators only" });
        }

        public ActionResult ControlPanel()
        {
            User user = db.Users.Find(Convert.ToInt32(Session["UserID"]));
            ViewBag.Address = user.Address;
            return View();
        }

        public ActionResult ManageUsers()
        {
            List<Models.User> users = db.Users.ToList();
            return View(users);
        }


        public ActionResult Statistics()
        {

            ViewBag.CommentsPerUser = GetUserComments();
            ViewBag.LikesPerUser = GetUserLikes();
            return View();
        }

        public ActionResult AdvancedSearch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchUsers(User user)
        {
            if (user.FirstName == null)
                user.FirstName = "";

            if (user.LastName == null)
                user.LastName = "";

            if (user.Email == null)
                user.Email = "";

            List<User> users = db.Users.Where(u => u.FirstName.Contains(user.FirstName) &&
                                                  u.LastName.Contains(user.LastName) &&
                                                  u.Email.Contains(user.Email)).ToList();

            if (users.Count == 0 || user == null)
                return View("NoResults");

            return View(users);
        }

        [HttpPost]
        public ActionResult SearchPosts(Post post)
        {
            if (post.Title == null)
                post.Title = "";

            if (post.Content == null)
                post.Content = "";

            List<Post> posts = db.Posts.Where(p => p.Content.Contains(post.Content) &&
                                                  p.Title.Contains(post.Title) &&
                                                  p.Likes >= post.Likes).ToList();

            if (posts.Count == 0 || posts == null)
                return View("NoResults");

            return View(posts);
        }

        [HttpPost]
        public ActionResult SearchGroups(Group group)
        {
            if (group.Title == null)
                group.Title = "";

            List<Group> groups = db.Groups.Where(g => g.Title.Contains(group.Title)).ToList();

            if (groups.Count == 0 || groups == null)
                return View("NoResults");

            return View(groups);
        }

        private List<string[]> GetUserComments()
        {
            var joinUsersComments = (from u in db.Users
                                     join c in db.Comments on u.UserID equals c.UserId
                                     select new { u.FirstName, u.LastName });

            var commentsPerUser = (from r in joinUsersComments
                                   group r by new { r.FirstName, r.LastName } into g
                                   select new { g.Key.FirstName, g.Key.LastName, Comments = g.Count() }).ToList();



            List<string[]> commentsList = new List<string[]>();

            foreach (var comment in commentsPerUser)
                commentsList.Add(new string[] { comment.FirstName, comment.LastName, comment.Comments.ToString() });

            return commentsList;
        }

        private List<string[]> GetUserLikes()
        {
            var joinUsersPosts = (from u in db.Users
                                  join p in db.Posts on u.UserID equals p.UserId
                                  select new { u.FirstName, u.LastName, p.Likes });


            var likesPerUser = (from r in joinUsersPosts
                                group r by new { r.FirstName, r.LastName } into g
                                select new { g.Key.FirstName, g.Key.LastName, Likes = g.Sum(r => r.Likes) }).ToList();

            List<string[]> likesList = new List<string[]>();

            foreach (var like in likesPerUser)
                likesList.Add(new string[] { like.FirstName, like.LastName, like.Likes.ToString() });

            return likesList;
        }
       
    }
}