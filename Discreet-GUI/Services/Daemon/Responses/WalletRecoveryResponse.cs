using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.Responses
{
    public class WalletRecoveryResponse
    {
        public string Seed { get; set; }
        public string Mnemonic { get; set; }
    }
}
