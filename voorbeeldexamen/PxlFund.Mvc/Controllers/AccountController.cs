using Microsoft.AspNetCore.Mvc;
using PxlFund.Shared.Services.Interfaces;

using PxlFund.Shared.Models;
using PxlFund.Shared.Requests;
using Microsoft.AspNetCore.Identity;

namespace PxlFund.Mvc.Controllers
{
    public class AccountController : Controller
    {
        IUserLoginRepository _loginRepo;
        public AccountController(IUserLoginRepository loginRepo)
        {
            _loginRepo = loginRepo;
        }
        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var result = _loginRepo.Login(login);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
            }
            return View();
        }
        #endregion
        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var result = _loginRepo.Register(registerModel);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
            }
            return View();
        }
        #endregion
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }
        #region External login
        public IActionResult ExternalLogin()
        {
            string redirectUrl = Url.Action("DuendeResponse", "Account");
            string scheme = "oidc";
            var properties = _loginRepo.ConfigureExternalAuthenticationProperties(
            scheme, redirectUrl);
            return new ChallengeResult(scheme, properties);
        }

        public
        #endregion
    }
}
