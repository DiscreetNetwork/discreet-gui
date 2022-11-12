using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.SeedRecovery.Responses
{
    public class GetWalletSeedResponse
    {
        public string Seed { get; set; }
        public string Mnemonic { get; set; }
    }
}
