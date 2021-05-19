using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyChat.Core;
using MyChat.Models;
using System;
using System.Diagnostics;


namespace MyChat.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> um;
        private readonly IMessageRepository messageRepository;

        public HomeController(ILogger<HomeController> logger,UserManager<AppUser> userManager,IMessageRepository messageRepository)
        {
            um = userManager;
            this.messageRepository = messageRepository;
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
        public JsonResult GetMessages(int skipCount, int takeCount)
        {
            return Json(messageRepository.GetMessages(skipCount, takeCount));
        }
        public IActionResult SaveMessage(Message message)
        {
            messageRepository.AddMessage(message);
            return Ok();
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
