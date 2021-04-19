using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyChat.Data;
using MyChat.Models;
using MyChat.Models.Account;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyChat.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext db;
        IWebHostEnvironment _appEnvironment;

        public AccountController(UserManager<User> userManager,SignInManager<User> singInManager,AppDbContext context, IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _signInManager = singInManager;
            _appEnvironment = appEnvironment;
            db = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            User u = new User();

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
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.UserName,Email = model.Email};
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
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Welcome", "Home");
        }

        public async Task<IActionResult> SetAvatar(IFormFile uploadedFile)
        {
            User u = _userManager.FindByNameAsync(User.Identity.Name).Result;
            string filename = u.UserName + uploadedFile.FileName.Substring(uploadedFile.FileName.LastIndexOf('.'));
            if (uploadedFile != null)
            {
                string path = _appEnvironment.WebRootPath + "/Avatars/" + filename;
                if(System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                u.AvatarPath = filename;
                
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Profile","Home");
        }
        /*public async Task<IActionResult> ChangeAvatar(ChangeAvatar ca)
        {
            User u = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if (ca != null)
            {
                byte[] avatar = null;
                using (var binaryReader = new BinaryReader(ca.Avatar.OpenReadStream()))
                {
                    avatar = binaryReader.ReadBytes((byte)u.Avatar.Length);
                }
                u.Avatar = avatar;
            }
            
            await db.SaveChangesAsync();

            return View("Profile", "Home");
        }*/

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
            return View("Settings",_userManager.FindByNameAsync(User.Identity.Name));
        }
    }
}
