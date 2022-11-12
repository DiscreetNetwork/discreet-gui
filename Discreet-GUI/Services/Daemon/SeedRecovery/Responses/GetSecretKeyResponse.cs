using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.SeedRecovery.Responses
{
    public class GetSecretKeyResponse
    {
        public string Spend { get; set; }
        public string View { get; set; }
        public string MnemonicSpend { get; set; }
        public string MnemonicView { get; set; }
        public string Secret { get; set; }
        public string Mnemonic { get; set; }
    }
}
