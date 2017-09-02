using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.DAL;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class PostsController : Controller
    {
        private NetworkContext db = new NetworkContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserID"] != null)
                base.OnActionExecuting(filterContext);
            else
                filterContext.Result = RedirectToAction("Login", "Welcome");
        }

        // GET: Posts/Create
        public ActionResult Create(int? groupId)
        {
            if (groupId == null)
            {
                groupId = 0;
            }
            ViewBag.GroupId = groupId;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupId,Title,Content")] Post post)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.Find(Convert.ToInt32(Session["UserID"]));
                post.User = user;
                post.PostDate = DateTime.Now;

                if (post.GroupId == 0)
                {
                    post.GroupId = null;
                    db.Posts.Add(post);
                    db.SaveChanges();
                    return RedirectToAction("Home", "Users");
                }

                Group group = db.Groups.Find(post.GroupId);
                group.Posts.Add(post);
                db.Entry(group).State = group.GroupID == 0 ?
                                   EntityState.Added :
                                   EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("ViewGroup", "Groups", new { groupId = group.GroupID });
            }

            return View(post);
        }

        public ActionResult Edit(int? postId)
        {

            if (postId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(postId);

            if (post == null)
            {
                return HttpNotFound();
            }

            if (!IsAuthorizedToEdit(post.PostID))
                return RedirectToAction("AccessDenied", "Welcome", new { ErrorMessage = "You are not authorized to edit this post" });

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupId,PostID,UserId,Title,Content,PostDate,Likes")] Post post)                                                 
        {
            if(!IsAuthorizedToEdit(post.PostID))
                return RedirectToAction("AccessDenied", "Welcome", new { ErrorMessage = "You are not authorized to edit this post" });

            if (ModelState.IsValid)
            {
                db.Entry(post).State = post.PostID == 0 ?
                                   EntityState.Added :
                                   EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Home","Users");
            }
            return View(post);
        }

        public ActionResult Delete(int? postId)
        {
            if (postId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(postId);

            if (post == null)
            {
                return HttpNotFound();
            }

            if (!IsAuthorizedToEdit(post.PostID))
                return RedirectToAction("AccessDenied", "Welcome", new { ErrorMessage = "You are not authorized to edit this post" });

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int postId)
        {
            Post post = db.Posts.Find(postId);

            if (!IsAuthorizedToEdit(post.PostID))
                return RedirectToAction("AccessDenied", "Welcome", new { ErrorMessage = "You are not authorized to edit this post" });

            if (post.Comments != null)
            {
                db.Comments.RemoveRange(post.Comments);

            }
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Home","Users");
        }

        [HttpPost]
        public int AddLike(int? postId)
        {

            Post post = db.Posts.Find(postId);
            post.Likes++;

            db.Entry(post).State = post.PostID == 0 ?
                                   EntityState.Added :
                                   EntityState.Modified;

            db.SaveChanges();
            return post.Likes;

        }

        private bool IsAuthorizedToEdit(int id)
        {
            Post post = db.Posts.AsNoTracking().Where(p => p.PostID == id).FirstOrDefault();

            if (post.User.UserID == GetUserIdFromSession())
                return true;
            else if (isAdmin())
                return true;

            return false;
        }

        private int GetUserIdFromSession()
        {
            return Convert.ToInt32(Session["UserID"]);
        }

        private bool isAdmin()
        {
            return Session["Admin"] != null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
