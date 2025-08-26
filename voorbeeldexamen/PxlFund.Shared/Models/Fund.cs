using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PxlFund.Shared.Models
{
    public class Fund
    {
        public int FundId { get; set; }
        public int BankId { get; set; }
        public string? FundName { get; set; }
        public decimal FundValue { get; set; }
        public Bank? Bank { get; set; }
        public ICollection<UserFund>? UserFunds { get; set; }

    }
}
