using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyChat.Data;
using MyChat.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyChat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> um;

        public HomeController(ILogger<HomeController> logger,UserManager<User> userManager)
        {
            _logger = logger;
            um = userManager;
        }

        public IActionResult Welcome()
        {
            return View("Welcome");
        }

        [Authorize]
        public IActionResult Profile(string name)
        {
            User u;
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
        public IActionResult Chat()
        {
            return View("Chat");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
