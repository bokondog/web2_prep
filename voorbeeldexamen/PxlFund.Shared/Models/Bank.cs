using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PxlFund.Shared.Models
{
    public class Bank
    {
        public int BankId { get; set; }
        public string? BankName { get; set; }
        public ICollection<Fund>? Funds { get; set; }
    }
}
