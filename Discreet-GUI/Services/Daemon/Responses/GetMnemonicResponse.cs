using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Services.Daemon.Common;

namespace Services.Daemon.Responses
{
    public class GetMnemonicResponse : DaemonErrorResult
    {
        public string Mnemonic { get; set; }
        public string Entropy { get; set; }
    }
}
