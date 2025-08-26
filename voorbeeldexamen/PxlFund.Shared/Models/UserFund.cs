using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PxlFund.Shared.Models
{
    public class UserFund
    {
        public int UserFundId { get; set; }
        public string? UserId { get; set; }
        public int FundId { get; set; }
        public int Amount { get; set; }
        public Fund? Fund { get; set; }

    }
}
