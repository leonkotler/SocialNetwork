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
    public class CommentsController : Controller
    {
        private NetworkContext db = new NetworkContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserID"] != null)
                base.OnActionExecuting(filterContext);
            else
                filterContext.Result = RedirectToAction("Login", "Welcome");
        }

        public ActionResult Create(int? postId)
        {    
            ViewBag.postId = postId;
            return View();
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create ([Bind(Include = "Content,PostId")] Comment comment)
        {
            User user = db.Users.Find(GetUserIdFromSession());
            Post post = db.Posts.Find(comment.PostId);

            if (ModelState.IsValid)
            {
                comment.User = user;
                comment.Post = post;
                db.Comments.Add(comment);
                db.SaveChanges();

                if (Session["GroupRedirectId"] != null)
                {
                    int SessionGroupId = Convert.ToInt32(Session["GroupRedirectId"]);
                    Session["GroupRedirectId"] = null;
                    return RedirectToAction("ViewGroup", "Groups", new { groupId = SessionGroupId });
                }

                return RedirectToAction("Home", "Users");
            }

            return View(comment);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            if (!IsAuthorizedToEdit(comment.CommentID))
                return RedirectToAction("AccessDenied", "Welcome", new { ErrorMessage = "You are not authorized to edit this post" });

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentID,PostID,UserID,Title,Content,Likes")] Comment comment)
        {
            if (!IsAuthorizedToEdit(comment.CommentID))
                return RedirectToAction("AccessDenied", "Welcome", new { ErrorMessage = "You are not authorized to edit this post" });

            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }


            if (!IsAuthorizedToEdit(comment.CommentID))
                return RedirectToAction("AccessDenied", "Welcome", new { ErrorMessage = "You are not authorized to edit this post" });

            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool IsAuthorizedToEdit(int id)
        {
            Comment comment = db.Comments.AsNoTracking().Where(c => c.CommentID == id).FirstOrDefault();

            if (comment.User.UserID == GetUserIdFromSession())
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
