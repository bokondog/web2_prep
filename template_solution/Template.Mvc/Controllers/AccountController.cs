using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Template.Mvc.Services;
using Template.Mvc.ViewModels;
using Template.Mvc.Models;
using Template.Mvc.Services.Interfaces;

namespace Template.Mvc.Controllers
{
    public class AccountController : Controller
    {
        UserRepository _userRepository;
        SignInManager<IdentityUser> _signInManager;
        IExternalAuthService _externalAuthService;
        public AccountController(UserRepository userRepository, SignInManager<IdentityUser> signInManager, IExternalAuthService externalAuthService)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _externalAuthService = externalAuthService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            var result = await _userRepository.LoginAsync(model.Email, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", result.Error!);
            }
            return View(model);
        }
        public IActionResult Register() { return View(); }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            var result = await _userRepository.RegisterAsync(model.FirstName, model.LastName, model.Email, model.Password, model.UserName);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("", result.Error!);
            }
            return View();
        }
        public IActionResult ExternalLogin()
        {
            string redirectUrl = Url.Action("ExternalLoginResponse", "Account");
            string scheme = "oidc";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(scheme, redirectUrl);
            return new ChallengeResult(scheme, properties);
        }

        public async Task<IActionResult> ExternalLoginResponse()
        {
            var result = await _externalAuthService.ExternalLogin();
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> LogOut()
        {
            var result = await _userRepository.LogoutAsync();
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Index", "Home");

        }
    }
}
