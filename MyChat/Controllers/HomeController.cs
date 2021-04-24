using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyChat.Data;
using MyChat.Models;
using System;
using System.Diagnostics;
using System.Linq;
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

        public IActionResult Test()
        {
            var mes =db.Messages;
            foreach(var m in mes)
            {
                string un = m.Sender.UserName;
            }
            return Ok();
        }

        [Authorize]
        public async Task<IActionResult> Chat()
        {
            var messages = db.Messages.OrderByDescending(m=> m.When).Take(30).Include(m=> m.Sender);
            return View("Chat",messages);
        }
        public async Task<IActionResult> Create(string sender, string text,string when)
        {
            var u = await um.GetUserAsync(User);
            Message m = new Message(u, text);
            db.Messages.Add(m);
            await db.SaveChangesAsync();
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
