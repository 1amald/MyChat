using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyChat.Controllers
{
    public class AccountController : Controller
    {
        private AppDbContext db;
        public AccountController(AppDbContext context)
        {
            db = context;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [Authorize]
        public IActionResult Profile(string id)
        {
            if(User.Identity.Name == id)
            {
                return View("MyProfile");
            }
            return View("AnotherProfile");
        }

        public IActionResult AddUser(string login,string email,string password,string confirmpass)
        {
            var u = db.Users.FirstOrDefault(user => user.Id == login);
            if (u != null)
            {
                return View("Exist");
            }
            if (password!= confirmpass)
            {
                return View("UncorrectPassword");
            }
            u = new User(login, password, email);
            db.Add(u);
            db.SaveChanges();
            return RedirectToAction("Profile","Account",u);
        }
        [HttpGet]
        public async Task<IActionResult> Login(string login,string password)
        {
            User user = await db.Users.FirstOrDefaultAsync(u => u.Id == login && u.Password == password);
            if (user != null)
            {
                await Authenticate(login); // аутентификация

                return RedirectToAction("Profile", "Account",user);
            }
            return View("Login");
        }

        private async Task Authenticate(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType,login)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
