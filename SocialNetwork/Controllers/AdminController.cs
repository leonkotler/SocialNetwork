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

        public ActionResult ControlPanel()
        {
            if (Session["Admin"] != null)
            {
                return View();
            }

            return RedirectToAction("AccessDenied", new { ErrorMessage = "Restricted to administrators only" });
        }

        public ActionResult ManageUsers()
        {
            if (Session["Admin"] != null)
            {
                List<Models.User> users = db.Users.ToList();
                return View(users);
            }

            return RedirectToAction("AccessDenied", new { ErrorMessage = "Restricted to administrators only" });
        }


        public ActionResult Statistics()
        {
            if (Session["Admin"] != null)
            {
                ViewBag.CommentsPerUser = GetUserComments();
                ViewBag.LikesPerUser = GetUserLikes();
                return View();
            }
            return RedirectToAction("AccessDenied", new { ErrorMessage = "Restricted to administrators only" });
        }

        public ActionResult AdvancedSearch()
        {
            if (Session["Admin"] != null)
                return View();

            return RedirectToAction("AccessDenied", new { ErrorMessage = "Restricted to administrators only" });
        }

        [HttpPost]
        public ActionResult SearchUsers(User user)
        {
            if (Session["Admin"] != null)
            {
                List<User> users = db.Users.Where(u => u.FirstName.Contains(user.FirstName) &&
                                                      u.LastName.Contains(user.LastName) &&
                                                      u.Email.Contains(user.Email)).ToList();

                if (users.Count == 0 || user == null)
                    return View("NoResults");

                return View(users);
            }
            return RedirectToAction("AccessDenied", new { ErrorMessage = "Restricted to administrators only" });
        }

        [HttpPost]
        public ActionResult SearchPosts(Post post)
        {
            if (Session["Admin"] != null)
            {
                List<Post> posts = db.Posts.Where(p => p.Content.Contains(post.Content) &&
                                                      p.Title.Contains(post.Title) &&
                                                      p.Likes >= post.Likes).ToList();

                if (posts.Count == 0 || posts == null)
                    return View("NoResults");

                return View(posts);
            }
            return RedirectToAction("AccessDenied", new { ErrorMessage = "Restricted to administrators only" });
        }

        [HttpPost]
        public ActionResult SearchGroups(Group group)
        {
            if (Session["Admin"] != null)
            {
                List<Group> groups = db.Groups.Where(g => g.Title.Contains(group.Title)).ToList();

                if (groups.Count == 0 || groups == null)
                    return View("NoResults");

                return View(groups);
            }
            return RedirectToAction("AccessDenied", new { ErrorMessage = "Restricted to administrators only" });
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

        public ActionResult AccessDenied(string ErrorMessage)
        {
            if (ErrorMessage != null)
            {
                ViewBag.ErrorMessage = ErrorMessage;
            }
            return View();
        }

    }
}