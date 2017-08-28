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

        public ActionResult Index()
        {
            var groups = db.Groups.ToList();
            ViewBag.Admins = adminMapper(groups);
            ViewBag.User = db.Users.Find(Convert.ToInt32(Session["UserID"]));
            return View(groups);
        }

        public ActionResult UserGroups()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
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

        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: Groups/Create
        public ActionResult Create(int? userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Groups/Edit/5
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

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupID,AdminId,Title,CreatedDate,Likes")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserGroups");
            }
            return View(group);
        }

        // GET: Groups/Delete/5
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
            return View(group);
        }

        // POST: Groups/Delete/5
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
