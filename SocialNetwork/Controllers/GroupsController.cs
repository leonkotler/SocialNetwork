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
    public class GroupsController : Controller
    {
        private NetworkContext db = new NetworkContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserID"] != null)
                base.OnActionExecuting(filterContext);
            else
                filterContext.Result = RedirectToAction("Login", "Welcome");
        }

        public ActionResult Index()
        {
            var groups = db.Groups.ToList();
            ViewBag.Admins = adminMapper(groups);
            ViewBag.User = db.Users.Find(GetUserIdFromSession());
            return View(groups);
        }

        public ActionResult UserGroups()
        {
            int userId = GetUserIdFromSession();
            User user = db.Users.Find(userId);
            ViewBag.User = user;

            var groups = user.Groups.ToList();
            ViewBag.Admins = adminMapper(groups);

            return View(groups);
        }

        public ActionResult ViewGroup(int? groupId)
        {
            Group group = db.Groups.Find(groupId);
            return View(group);
        }

        public ActionResult Create(int? userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdminId,Title")] Group group)
        {
            if (ModelState.IsValid)
            {
                group.CreatedDate = DateTime.Now;
                User groupMemberAlsoAdmin = db.Users.Find(group.AdminId);
                group.Members = new List<User>();
                group.Posts = new List<Post>();
                group.Members.Add(groupMemberAlsoAdmin);
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        public ActionResult Edit(int? groupId)
        {
            if (groupId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(groupId);
            if (group == null)
            {
                return HttpNotFound();
            }

            if (!IsAuthorizedToEdit(group.GroupID))
                return RedirectToAction("AccessDenied", "Welcome", new { ErrorMessage = "You are not authorized to edit this post" });

            return View(group);
        }

        public ActionResult AddMember(int groupId, int userId)
        {
            Group group = db.Groups.Find(groupId);
            User user = db.Users.Find(userId);

            group.Members.Add(user);
            db.Entry(group).State = group.GroupID == 0 ?
                                   EntityState.Added :
                                   EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("ViewGroup", new { groupId = groupId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupID,AdminId,Title,CreatedDate,Likes")] Group group)
        {
            if (!IsAuthorizedToEdit(group.GroupID))
                return RedirectToAction("AccessDenied", "Welcome", new { ErrorMessage = "You are not authorized to edit this post" });

            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserGroups");
            }
            return View(group);
        }

        public ActionResult Delete(int? groupId)
        {
            if (groupId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Group group = db.Groups.Find(groupId);

            if (group == null)
            {
                return HttpNotFound();
            }

            if (!IsAuthorizedToEdit(group.GroupID))
                return RedirectToAction("AccessDenied", "Welcome", new { ErrorMessage = "You are not authorized to edit this post" });

            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int groupId)
        {
            Group group = db.Groups.Find(groupId);

            if (group.Posts != null)
            {
                foreach (var post in group.Posts)
                {
                    if (post.Comments != null)
                    {
                        db.Comments.RemoveRange(post.Comments);                      
                    }
                }
                db.Posts.RemoveRange(group.Posts);
            }

            db.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void SetGroupSession(int? groupId)
        {
            Session["GroupRedirectId"] = groupId;
        }

        [HttpPost]
        public int AddLike(int? groupId)
        {

            Group group = db.Groups.Find(groupId);
            group.Likes++;

            db.Entry(group).State = group.GroupID == 0 ?
                                   EntityState.Added :
                                   EntityState.Modified;

            db.SaveChanges();
            return group.Likes;

        }

        private bool IsAuthorizedToEdit(int id)
        {
            Group group = db.Groups.AsNoTracking().Where(g => g.GroupID == id).FirstOrDefault();

            if (group.AdminId == GetUserIdFromSession())
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

        private Dictionary<int, string> adminMapper(List<Group> groups)
        {
            Dictionary<int, string> groupAdminNames = new Dictionary<int, string>();
            foreach (var group in groups)
            {
                User admin = db.Users.Find(group.AdminId);
                string userName = admin.FirstName + " " + admin.LastName;
                if (!groupAdminNames.ContainsKey(group.AdminId))
                {
                    groupAdminNames.Add(group.AdminId, userName);
                }
            }

            return groupAdminNames;
        }
    }
}
