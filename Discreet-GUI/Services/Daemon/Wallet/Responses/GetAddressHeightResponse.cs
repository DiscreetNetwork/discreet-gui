using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.Wallet.Responses
{
    public class GetAddressHeightResponse
    {
        public long Height { get; set; }
        public bool Synced { get; set; }
        public bool Syncer { get; set; }
    }
}
