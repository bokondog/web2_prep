using Microsoft.AspNetCore.Identity;
using Template.Mvc.Services.Results;

namespace Template.Mvc.Services.Interfaces
{
    public interface IUserRepository
    {
        Task<LoginResult> LoginAsync(string email, string password);
        Task<LogoutResult> LogoutAsync();
        Task<IdentityUser> GetIdentityUserAsync(string firstName, string lastName, string email);
        ExternalLoginResult ExternalLogin(string scheme, string? redirectUrl);
        Task<ExternalLoginResult> ExternalLoginInfoResponseAsync();
        Task<ExternalLoginResult> ExternalLoginInfoResponseAsync(ExternalLoginInfo externalLoginInfo);
    }
}
