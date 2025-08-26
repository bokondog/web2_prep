using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PxlFund.Shared.Requests
{
    public class RegisterModel : LoginModel
    {
        public string? UserName { get; set; }
    }
}
