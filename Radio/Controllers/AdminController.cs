namespace Radio.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Radio.Models;

    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("/Login")]
        public ViewResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost("/Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser targetUser = await _userManager.FindByNameAsync(login.Username);

            if (targetUser == null)
            {
                ModelState.AddModelError(nameof(login.Username), "Username does not exist.");
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(targetUser, login.Password, isPersistent: false, lockoutOnFailure: false);

            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl ?? "/admin");
            }

            ModelState.AddModelError(nameof(login.Password), "Incorrect password.");
            return View();
        }
    }
}