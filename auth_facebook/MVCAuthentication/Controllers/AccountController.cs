using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCAuthentication.ViewModels;
using System.Security.Claims;

namespace MVCAuthentication.Controllers
{
    
    public class AccountController : Controller
    {
        UserManager<IdentityUser> _userManager;
        SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = model.UserName;
                user.Email = model.Email;
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.USER_ROLE);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(model);
                }
            }
            return View(model);
        }
        #endregion
        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(UserViewModel model)
        {            
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var searchUser = await _userManager.FindByNameAsync(model.UserName);
                    if (searchUser != null)
                        await _signInManager.SignInAsync(searchUser, isPersistent: false);
                    return RedirectToAction("Index", "Test");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt");
                }
            }
            return View("Login", model);
        }

        public IActionResult FacebookLogin()
        {
            string redirectUrl = Url.Action("FacebookResponse", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook", properties);
        }

        public async Task<IActionResult> FacebookResponse()
        {
            ExternalLoginInfo externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                return RedirectToAction(nameof(Login));
            }

            string userName = externalLoginInfo.Principal.FindFirst(ClaimTypes.Name).Value;
            UserViewModel model = new UserViewModel()
            {
                UserName = userName,
                Email = externalLoginInfo.Principal.FindFirst(ClaimTypes.Email).Value
            };

            Microsoft.AspNetCore.Identity.SignInResult result = 
                await _signInManager.ExternalLoginSignInAsync(
                    externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, false);
            if (!result.Succeeded)
            {
                var identityResult = await CreateIdentityUserAsync(externalLoginInfo);
                if (!identityResult.Succeeded) {
                    return View("Login");
                }
            }

            return View(model);
        }

        private IdentityUser GetIdentityUser(ExternalLoginInfo info)
        {
            string userName = info.Principal.FindFirst(ClaimTypes.Name).Value;
            userName = $"{userName}_{info.LoginProvider}_{info.ProviderKey}";
            string email = info.Principal.FindFirst(ClaimTypes.Email).Value;
            IdentityUser user = new IdentityUser(userName)
            {
                Email = email
            };
            return user;
        }

        private async Task<IdentityResult> CreateIdentityUserAsync(ExternalLoginInfo externalLoginInfo)
        {
            IdentityUser user = GetIdentityUser(externalLoginInfo);
            IdentityResult identityResult = await _userManager.CreateAsync(user);
            if (identityResult.Succeeded)
            {
                identityResult = await _userManager.AddLoginAsync(user, externalLoginInfo);
                if (identityResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                }
                else
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Error in Addlogin" });
                }
            }
            return identityResult;
        }

        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo externalLoginInfo =
                await _signInManager.GetExternalLoginInfoAsync();

            if (externalLoginInfo == null)
            {
                return RedirectToAction(nameof(Login));
            }

            string userName = externalLoginInfo.Principal.FindFirst(ClaimTypes.Name).Value;
            UserViewModel model = new UserViewModel()
            {
                UserName = userName,
                Email = externalLoginInfo.Principal.FindFirst(ClaimTypes.Email).Value
            };

            Microsoft.AspNetCore.Identity.SignInResult result =
                await _signInManager.ExternalLoginSignInAsync(
                    externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, false);
            if (!result.Succeeded)
            {
                var identityResult = await CreateIdentityUserAsync(externalLoginInfo);
                if (!identityResult.Succeeded)
                {
                    return View("Login");

                }
            }
            return View(model);
        }

        #endregion

    }
}
