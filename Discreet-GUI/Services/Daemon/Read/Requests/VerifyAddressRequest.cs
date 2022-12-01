using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.Read.Requests
{
    public class VerifyAddressRequest
    {
        public string Address { get; set; }

        public VerifyAddressRequest(string address)
        {
            Address = address;
        }
    }
}
