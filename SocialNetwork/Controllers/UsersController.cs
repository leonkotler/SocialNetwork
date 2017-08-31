using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SocialNetwork.DAL;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class UsersController : Controller
    {
        private NetworkContext db = new NetworkContext();

        // Will execute before any action
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserID"] != null)
                base.OnActionExecuting(filterContext);
            else
                filterContext.Result = RedirectToAction("Login", "Welcome");
        }

        public ActionResult Home()
        {
            var allPosts = db.Posts.OrderByDescending(p => p.Likes).ToList();
            return View(allPosts);
        }

        public ActionResult UserProfile(int? userId)
        {
            if (userId == null || db.Users.Find(userId) == null)
                userId = GetUserIdFromSession();

            var userPosts = db.Posts.Where(p => p.UserId == userId).OrderByDescending(p => p.Likes).ToList();

            if (userPosts != null)
                return View(userPosts);

            else return View();
        }

        public ActionResult Edit(int? userId)
        {
            if (userId == null || db.Users.Find(userId) == null)
                userId = GetUserIdFromSession();

            else if (userId != GetUserIdFromSession() && Session["Admin"] == null)
                return RedirectToAction("AccessDenied", "Welcome", new { ErrorMessage = "Access Denied" });

            User user = db.Users.Find(userId);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,Gender,Email,BirthDate,Password,Address")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Logout", "Welcome");
            }

            return View(user);
        }

        private int GetUserIdFromSession()
        {
            return Convert.ToInt32(Session["UserID"]);
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
