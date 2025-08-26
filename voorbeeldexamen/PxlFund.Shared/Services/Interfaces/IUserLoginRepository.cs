using PxlFund.Shared.Requests;
using PxlFund.Shared.Services.Results;

namespace PxlFund.Shared.Services.Interfaces
{
    public interface IUserLoginRepository
    {
        UserLoginResult Register(RegisterModel register);
        UserLoginResult Login(LoginModel login);
    }
}
