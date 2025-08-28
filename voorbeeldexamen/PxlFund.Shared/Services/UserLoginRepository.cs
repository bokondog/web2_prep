using Microsoft.AspNetCore.Identity;
using PxlFund.Shared.Requests;
using PxlFund.Shared.Services.Interfaces;
using PxlFund.Shared.Services.Results;
using PxlFund.Mvc.Data;


namespace PxlFund.Shared.Services
{
    public class UserLoginRepository : IUserLoginRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly ApplicationDbContext _context;
        public UserLoginRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager)
        {
            _context = context;
            _userManager = userManager;
            _signinManager = signinManager;
        }
        public UserLoginResult Register(RegisterModel register)
        {
            return Register(register.UserName, register.Email, register.Password);
        }
        public UserLoginResult Register(string userName, string email, string password)
        {
            var result = new UserLoginResult();
            var user = new IdentityUser { UserName = userName, Email = email };
            var registerResult = _userManager.CreateAsync(user, password);
            if (registerResult.Result.Succeeded)
            {
                result.Success();
            }
            else
            {
                result.Error = "Error while trying to register this user!";
            }
            return result;
        }
        public UserLoginResult Login(LoginModel login)
        {
            return Login(login.Email, login.Password);
        }
        public UserLoginResult Login(string email, string password)
        {
            var result = new UserLoginResult();
            var user = GetIdentityUser(email);
            if (user != null)
            {
                var loginResult = _signinManager.PasswordSignInAsync(user, password, false, lockoutOnFailure: false);
                if (loginResult.Result.Succeeded)
                {
                    result.Success();
                }
                else
                {
                    result.Failed("Passwordwas not correct for this user!");
                }
            }
            else
            {
                result.Failed("No user found for this email address!");
            }
            return result;
        }

        UserLoginResult IUserLoginRepository.Register(RegisterModel register)
        {
            throw new NotImplementedException();
        }

        UserLoginResult IUserLoginRepository.Login(LoginModel login)
        {
            throw new NotImplementedException();
        }
        private IdentityUser GetIdentityUser(string email)
        {
            IdentityUser user = null;
            user = _userManager.FindByEmailAsync(email).Result;
            return user;
        }
        public SignOutResult Signout()
        {
            var result = new SignOutResult();
            try
            {
                _signinManager.SignOutAsync();
                result.Success();
            }
            catch
            { }
            return result;
        }
    }
}
