namespace Radio.ApiControllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Radio.Extensions;
    using Radio.Models.User;
    using Radio.Services;

    using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

    [ApiController]
    [Route("[Controller]/[Action]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly IUserImageService _userImageService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IUserImageService userImageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userImageService = userImageService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(Registration registration)
        {
            if (ModelState.IsValid)
            {
                AppUser newUser = new AppUser
                {
                    UserName = registration.Username
                };

                IdentityResult createResult = await _userManager.CreateAsync(newUser, registration.Password);

                if (createResult.Succeeded)
                {
                    await _userImageService.StoreImageForUserId(newUser.Id);

                    await _signInManager.SignInAsync(newUser, isPersistent: true);
                    return Ok();
                }

                ModelState.AddIdentityErrors(createResult);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(login.Username);

                if (user == null)
                {
                    ModelState.AddModelError("Username", "Username is not registered.");
                }
                else
                {
                    SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, isPersistent: true, lockoutOnFailure: false);

                    if (signInResult.Succeeded)
                    {
                        return Ok();
                    }

                    ModelState.AddModelError("Password", "Incorrect password.");
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}