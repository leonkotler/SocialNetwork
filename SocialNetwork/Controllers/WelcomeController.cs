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
    public class WelcomeController : Controller
    {
        private NetworkContext db = new NetworkContext();

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
                SetSessionForUser(loggedUser);
                SetUserImage(loggedUser);
                return RedirectToAction("Home","Users");
            }

            else
                ModelState.AddModelError("", "Wrong credentials");

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "UserID,FirstName,LastName,Password,Gender,Email,BirthDate,ImageUrl,IsAdmin,Address")] User user)
        {
            if (ModelState.IsValid)
            {
                if (EmailAlreadyTaken(user.Email))
                {
                    ViewBag.ErrorMsg = "Email already taken";
                    return View(user);
                }

                user.IsAdmin = false;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Home", "Users");
            }

            return View(user);
        }

        private bool EmailAlreadyTaken(string email)
        {
            User user = db.Users.AsNoTracking().Where(u => u.Email == email).FirstOrDefault();

            return (user != null);
        }

        public ActionResult AccessDenied(string ErrorMessage)
        {
            if (ErrorMessage != null)
            {
                ViewBag.ErrorMessage = ErrorMessage;
            }
            return View();
        }

        private void SetSessionForUser(User loggedUser)
        {
            Session["UserID"] = loggedUser.UserID.ToString();
            Session["Username"] = loggedUser.Email.ToString();
            Session["Fullname"] = loggedUser.FirstName.ToString() + " " + loggedUser.LastName.ToString();

            if (loggedUser.IsAdmin)
                Session["Admin"] = "true";
        }

        private void SetUserImage(User loggedUser)
        {
            if (loggedUser.ImageUrl != null)
                Session["ImageUrl"] = loggedUser.ImageUrl;

            else if (loggedUser.Gender == Utils.Utils.MyGender.Male)
                Session["ImageUrl"] = "/Content/images/male.png";

            else
                Session["ImageUrl"] = "/Content/images/female.png";
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
