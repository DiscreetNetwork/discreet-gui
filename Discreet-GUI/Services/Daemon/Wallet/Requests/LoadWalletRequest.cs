﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.Wallet.Requests
{
    public class LoadWalletRequest
    {
        public string Label { get; set; }
        public string Passphrase { get; set; }
    }
}
