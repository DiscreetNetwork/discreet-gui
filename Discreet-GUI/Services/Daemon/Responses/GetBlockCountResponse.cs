using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Daemon.Responses
{
    public class GetBlockCountResponse
    {
        public long Height { get; set; }
        public string Status { get; set; }
        public bool Untrusted { get; set; }
        public bool Synced { get; set; }
    }
}
