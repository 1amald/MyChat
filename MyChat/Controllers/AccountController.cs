using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyChat.Data;
using MyChat.Models;
using MyChat.Models.Account;
using System.Linq;
using System.Threading.Tasks;

namespace MyChat.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext db;
        public AccountController(UserManager<User> userManager,SignInManager<User> singInManager,AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = singInManager;
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
                u= db.Users.FirstOrDefault(user => user.Id == model.LoginOrEmail);
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
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Welcome", "Home");
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
    }
}
