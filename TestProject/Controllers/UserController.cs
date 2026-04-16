using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProject.Models;

namespace TestProject.Controllers
{
    public class UserController : Controller
    {
        AssignmentEntities context = new AssignmentEntities();
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string username)
        {
            if (IsUsernameExists(username))
            {
                ViewBag.ErrorMessage = "Username already exists. Choose a different username.";
                return View();
            }
            string generatedPassword = GetGeneratePassword(username);

            var user = new UserCredential { UserName = username, Password = generatedPassword };
            context.UserCredentials.Add(user);
            context.SaveChanges();

            return RedirectToAction("Login", new { username = username, generatedPassword = generatedPassword });
        }

        public ActionResult Login(string username, string generatedPassword)
        {
            var model = new UserModel
            {
                UserName = username,
                GeneratedPassword = generatedPassword
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Login(UserModel model)
        {
            var user = context.UserCredentials.FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.GeneratedPassword);
            
            if (user != null)
            {
                return RedirectToAction("CreateOrder","Restaurant");
            }
            else
            {
                return View(model);
            }
            
        }
        private string GetGeneratePassword(string username)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string reversedUsername = new string(username.Reverse().ToArray());
            string firstAndLastLetters = username[0].ToString() + username[username.Length - 1];

            return reversedUsername + timestamp + firstAndLastLetters;
        }
        [HttpPost]
        public JsonResult IsUsernameAvailable(string username)
        {
            bool isUsernameAvailable = !context.UserCredentials.Any(u => u.UserName == username);
            Response.Cache.SetNoStore();
            return Json(isUsernameAvailable, JsonRequestBehavior.AllowGet);
        }
        private bool IsUsernameExists(string username)
        {
            return context.UserCredentials.Any(u => u.UserName == username);
        }

    }
}
