using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Template.Mvc.Services.Results
{
    public class ExternalLoginResult : BaseResult
    {
        public AuthenticationProperties? AuthenticationProperties { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public IdentityUser? IdentityUser { get; set; }
    }
}