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
using System.Web.Security;

namespace SocialNetwork.Controllers
{
    public class UsersController : Controller
    {
        private NetworkContext db = new NetworkContext();

        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users
        public ActionResult Home(User loggedUser)
        {
            if (Session["UserID"] != null)
            {
                var userPosts = db.Posts.Where(p => p.UserID == loggedUser.UserID).ToList();
                return View(userPosts);
            }
            else return RedirectToAction("Login");
        }

        // GET: Users/Register
        public ActionResult Register()
        {
            return View();
        }

        // GET: Users/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var loggedUser = db.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
            if (loggedUser != null)
            {
                Session["UserID"] = loggedUser.UserID.ToString();
                Session["Username"] = loggedUser.Email.ToString();
                return RedirectToAction("Home", loggedUser);
            }
            else
            {
                ModelState.AddModelError("", "Wrong credentials");
            }
            return View();
        }


        [HttpPost]
        public ActionResult Register(User account)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(account);
                db.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.Message = account.FirstName + " " + account.LastName + "successfuly registered!";

            return View();
        }

       
        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,FirstName,LastName,Gender,Email,BirthDate")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
