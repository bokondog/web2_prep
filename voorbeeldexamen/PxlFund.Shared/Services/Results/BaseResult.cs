using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PxlFund.Shared.Services.Results
{
    public abstract class BaseResult
    {
        private bool _succeeded = false;
        public bool Succeeded => Succeeded;
        public string? Error { get; set; }
        public void Failed(string error)
        {
            _succeeded = false;
            Error = error;
        }
        public void Success()
        {
            _succeeded = true;
        }
    }
}
