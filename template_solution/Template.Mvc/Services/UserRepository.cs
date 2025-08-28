using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Template.Mvc.Services.Interfaces;
using Template.Mvc.Data;
using Template.Mvc.Services.Results;
using Template.Mvc.Models;

namespace Template.Mvc.Services
{
    public class UserRepository : IUserRepository
    {
        SignInManager<IdentityUser> _signInManager;
        UserManager<IdentityUser> _userManager;
        TemplateDbContext _templateDbContext;
        public UserRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, TemplateDbContext templateDbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _templateDbContext = templateDbContext;
        }
        #region Register
        public async Task<LoginResult> RegisterAsync(string firstName, string lastName, string email, string password, string userName)
        {
            var result = new LoginResult();
            var user = new IdentityUser { UserName = userName, Email = email };

            try
            {
                var registerResult = _userManager.CreateAsync(user, password);
                if (registerResult.Result.Succeeded)
                {
                    user = await _userManager.FindByEmailAsync(email);
                    TemplateUser templateUser = new TemplateUser()
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        IdentityUserId = user.Id
                    };
                    if (user != null)
                    {
                        _templateDbContext.TemplateUsers.Add(templateUser);
                        await _templateDbContext.SaveChangesAsync();

                    }
                    result.Success();
                }
                else
                {
                    result.Failed("Error while trying to register this user!");
                }
                return result;

            }
            catch (Exception ex)
            {
                result.Failed(ex.Message);
            }
            return result;
        }

        #endregion
        #region Login
        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            var result = new LoginResult();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var signinResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                    if (signinResult.Succeeded)
                    {
                        result.Success();
                    }
                    else
                    {
                        result.Failed("Problems signing in!");
                    }
                }
                else
                {
                    result.Failed("User not found!");
                }
            }
            catch (Exception ex)
            {
                result.Failed(ex.Message);
            }
            return result;
        }

        #endregion
        #region Logout
        public async Task<LogoutResult> LogoutAsync()
        {
            var result = new LogoutResult();
            try
            {
                await _signInManager.SignOutAsync();
                result.Success();
            }
            catch (Exception ex)
            {
                result.Failed(ex.Message);
            }
            return result;
        }
        #endregion
        #region external login
        public async Task<IdentityUser> GetIdentityUserAsync(string firstName, string lastName, string email)
        {

            var dbUser = await _userManager.FindByEmailAsync(email);
            if (dbUser != null)
            {
                return dbUser;
            }
            var user = new IdentityUser();
            user.UserName = $"{firstName}_{lastName}";
            user.Email = email;
            return user;
        }
        public ExternalLoginResult ExternalLogin(string scheme, string? redirectUrl)
        {
            var result = new ExternalLoginResult();
            try
            {

            }
            catch (Exception ex)
            {
                result.Failed(ex.Message);
            }
            return result;
        }
        public async Task<ExternalLoginResult> ExternalLoginInfoResponseAsync()
        {
            var result = new ExternalLoginResult();
            try
            {

            }
            catch (Exception ex)
            {
                result.Failed(ex.Message);
            }
            return result;
        }
        public async Task<ExternalLoginResult> ExternalLoginInfoResponseAsync(ExternalLoginInfo externalLoginInfo)
        {
            var result = new ExternalLoginResult();
            try
            {

            }
            catch (Exception ex)
            {
                result.Failed(ex.Message);
            }
            return result;
        }


        #endregion
    }
}
