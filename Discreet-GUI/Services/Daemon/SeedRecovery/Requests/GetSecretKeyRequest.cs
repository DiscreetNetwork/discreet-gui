using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.SeedRecovery.Requests
{
    public class GetSecretKeyRequest
    {
        public string Label { get; set; }
        public string Passphrase { get; set; }
        public string Address { get; set; }
    }
}
