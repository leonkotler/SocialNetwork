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

 
        // GET: Comments/Create
        public ActionResult Create(int? postId)
        {    
            ViewBag.postId = postId;
            return View();
        }

        // POST: Comments/Create      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create ([Bind(Include = "Content,PostId")] Comment comment)
        {
            User user = db.Users.Find(Convert.ToInt32(Session["UserID"]));
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

        // GET: Comments/Edit/5
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
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentID,PostID,UserID,Title,Content,Likes")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
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
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
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
