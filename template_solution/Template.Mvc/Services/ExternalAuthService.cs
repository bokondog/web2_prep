using Microsoft.AspNetCore.Identity;
using Template.Mvc.Services.Interfaces;
using Template.Mvc.Services.Results;
using System.Security.Claims;

namespace Template.Mvc.Services
{
    public class ExternalAuthService : IExternalAuthService
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public ExternalAuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<LoginResult> ExternalLogin()
        {
            var result = new LoginResult();

            ExternalLoginInfo? externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                result.Success();
                return result;
            }
            SignInResult signinResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, false);
            if (!signinResult.Succeeded)
            {
                var createResult = await CreateIdentityUserAsync(externalLoginInfo);
                if (createResult.Succeeded)
                {
                    result.Success();
                }
                else
                {
                    result.Success();
                    return result;
                }
            }
            result.Success();
            return result;
        }
        public async Task<IdentityResult> CreateIdentityUserAsync(ExternalLoginInfo externalLoginInfo)
        {
            // Create IdentityUser based on claims from Facebook
            IdentityUser user = GetIdentityUser(externalLoginInfo);

            // Try to create the user in the database
            IdentityResult identityResult = await _userManager.CreateAsync(user);

            if (identityResult.Succeeded)
            {
                // Link the external Facebook login to the created user
                identityResult = await _userManager.AddLoginAsync(user, externalLoginInfo);

                if (identityResult.Succeeded)
                {
                    // Sign in the user if everything succeeded
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                else
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Error in AddLogin" });
                }
            }

            return identityResult;
        }
        public IdentityUser GetIdentityUser(ExternalLoginInfo info)
        {
            string email = info.Principal.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                email = info.Principal.FindFirst("email").Value;
            }
            IdentityUser user = new IdentityUser(email)
            {
                Email = email,
                UserName = email
            };
            return user;
        }
    }

}

