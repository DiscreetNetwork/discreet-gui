using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Services.Daemon.Common;

namespace Services.Daemon.Wallet.Responses
{
    public class GetMnemonicResponse
    {
        public string Mnemonic { get; set; }
        public string Entropy { get; set; }
    }
}
