using Data;
using Entity;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace Demo.Web.Controllers
{
    public class SecurityUserController : Controller
    {
        private SecurityUserManager securityUserManager = new SecurityUserManager();

        /// <summary>
        /// Registration view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        /// <summary>
        /// Post of Registration view
        /// </summary>
        /// <param name="SecurityUser"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(SecurityUser SecurityUser)
        {
            bool Status = false;
            string message = "";

            //validate
            if (ModelState.IsValid)
            {
                //check if email already exists
                var isExist = securityUserManager.IsEmailExist(SecurityUser.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(SecurityUser);
                }

                //secure password with Hashing 
                SecurityUser.Password = Crypto.Hash(SecurityUser.Password);
                SecurityUser.ConfirmPassword = Crypto.Hash(SecurityUser.ConfirmPassword);
                SecurityUser.CreatedAt = DateTime.Now;

                //Save to Database
                securityUserManager.AddUser(SecurityUser);
                message = "Registration successfully done. Please login with your credentials";
                Status = true;
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(SecurityUser);
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(SecurityUserLogin login, string ReturnUrl = "")
        {
            string message = "";
            var v = securityUserManager.GetUserByEmail(login.Email);
            if (v != null)
            {
                if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                {
                    int timeout = login.RememberMe ? 3600 : 20;
                    var ticket = new FormsAuthenticationTicket(login.Email, login.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(ReturnUrl))
                        return Redirect(ReturnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                    message = "Invalid credential provided";
            }
            else
                message = "Invalid credential provided";

            ViewBag.Message = message;
            return View();
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

    }
}