using Microsoft.AspNetCore.Identity;
using Template.Mvc.Services.Results;

namespace Template.Mvc.Services.Interfaces
{
    public interface IExternalAuthService
    {
        Task<LoginResult> ExternalLogin();
        Task<IdentityResult> CreateIdentityUserAsync(ExternalLoginInfo externalLoginInfo);
        IdentityUser GetIdentityUser(ExternalLoginInfo info);

    }
}
