using Services.Daemon.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.Responses
{
    public class GetAddressHeightResponse: DaemonErrorResult
    {
        public long Height { get; set; }
        public bool Synced { get; set; }
        public bool Syncer { get; set; }
    }

    public class GetWalletHeightResponse: DaemonErrorResult
    {
        public long Height { get; set; }
        public bool Synced { get; set; }
    }
}
