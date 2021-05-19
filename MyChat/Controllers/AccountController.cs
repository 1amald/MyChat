using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyChat.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Identity.Core;
using MyChat.Core;
using MyChat.ViewModels;

namespace MyChat.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext db;
        IWebHostEnvironment _appEnvironment;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> singInManager,AppDbContext context, IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _signInManager = singInManager;
            _appEnvironment = appEnvironment;
            db = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            AppUser u = new AppUser();

            if (model.LoginOrEmail.Contains('@'))
            {
                u = db.Users.FirstOrDefault(user => user.NormalizedEmail == model.LoginOrEmail.ToUpper());
            }
            else
            {
                u= db.Users.FirstOrDefault(user => user.UserName == model.LoginOrEmail);
            }
            if (u != null)
            {
                var result = await _signInManager.PasswordSignInAsync(u, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", "Home");
                }
            }

            ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.UserName,Email = model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Profile", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View("Register",model);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Welcome", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> SetAvatar(string filename,IFormFile blob)
        {
            string userName = User.Identity.Name;
            AppUser u = await _userManager.FindByNameAsync(userName);
            string projectPath = "/Avatars/" + userName + ".jpg";
            if (blob != null)
            {
                string path = _appEnvironment.WebRootPath + projectPath;
                if(System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await blob.CopyToAsync(fileStream);
                }
                u.AvatarPath = projectPath;
                
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Profile","Home");
        }
        [Authorize]
        public async Task SetStatus(string status)
        {
            AppUser u = await _userManager.FindByNameAsync(User.Identity.Name);
            u.Status = status;
            await db.SaveChangesAsync();
        }
        [Authorize]
        public async Task<IActionResult> ChangePassword(string currentPassword,string newPassword,string confirmPassword)
        {
            if(newPassword != confirmPassword)
            {
                return BadRequest("Пароли не совпадают.");
            }
            if (newPassword.Length < 4)
            {
                return Json("Пароль должен быть не короче 4 символов.");
            }
            AppUser currentUser = await _userManager.GetUserAsync(User);
            IdentityResult result =
                await _userManager.ChangePasswordAsync(currentUser, currentPassword, newPassword);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Неверный текущий пароль.");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Profile", "Home");
            }
            return View("Register");
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Profile", "Home");
            }
            return View("Login");
        }
        [Authorize]
        [HttpGet]
        public IActionResult Settings()
        {
            return View("Settings");
        }
    }
}
