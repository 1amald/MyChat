using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyChat.Models;
using System.IO;
using System.Threading.Tasks;
using MyChat.ViewModels;

namespace MyChat.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        IWebHostEnvironment _appEnvironment;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> singInManager,IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _signInManager = singInManager;
            _appEnvironment = appEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong login or password");
                }
            }

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
                await _userManager.UpdateAsync(u);
            }
            return RedirectToAction("Profile","Home");
        }
        [Authorize]
        public async Task SetStatus(string status)
        {
            AppUser u = await _userManager.FindByNameAsync(User.Identity.Name);
            u.Status = status;
            await _userManager.UpdateAsync(u);
        }
        [Authorize]
        public async Task<IActionResult> ChangePassword(string currentPassword,string newPassword,string confirmPassword)
        {
            if(newPassword != confirmPassword)
            {
                return BadRequest("Password mismatch.");
            }
            if (newPassword.Length < 4)
            {
                return Json("Password must be at least 4 characters.");
            }
            AppUser currentUser = await _userManager.GetUserAsync(User);
            var result =
                await _userManager.ChangePasswordAsync(currentUser, currentPassword, newPassword);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Invalid current password.");
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
