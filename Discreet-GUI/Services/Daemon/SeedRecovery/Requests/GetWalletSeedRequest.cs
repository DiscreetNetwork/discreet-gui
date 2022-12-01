using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.SeedRecovery.Requests
{
    public class GetWalletSeedRequest
    {
        public string Label { get; set; }
        public string Passphrase { get; set; }
    }
}
