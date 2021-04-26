using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyChat.Data;
using MyChat.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyChat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> um;
        private AppDbContext db;

        public HomeController(ILogger<HomeController> logger,UserManager<AppUser> userManager,AppDbContext context)
        {
            _logger = logger;
            um = userManager;
            db = context;
        }

        public IActionResult Welcome()
        {
            return View("Welcome");
        }

        [Authorize]
        public IActionResult Profile(string name)
        {
            AppUser u;
            if(name == null)
            {
                u = um.FindByNameAsync(User.Identity.Name).Result;
            }
            else
            {
                u = um.FindByNameAsync(name).Result;
            }
            if (u == null)
            {
                return RedirectToAction("Error");
            }
            return View("Profile",u);
        }
        [Authorize]
        public async Task<JsonResult> GetMoreMessages(int skipCount, int takeCount)
        {
            var messages = await db.Messages.OrderByDescending(m => m.When).Skip(skipCount).Take(takeCount).ToArrayAsync();
            var result = from m in messages
                         join u in db.Users on m.UserName equals u.UserName
                         select new { userName = m.UserName, text = m.Text, when = m.When,shortDate = m.When.ToShortTimeString(), avatarPath = u.AvatarPath };
                           
            return Json(result);
        }

        [Authorize]
        public IActionResult Chat()
        {
            //var messages = await db.Messages.OrderByDescending(m => m.When).Take(10).Include(m=> m.Sender).ToArrayAsync();
            return View("Chat");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
