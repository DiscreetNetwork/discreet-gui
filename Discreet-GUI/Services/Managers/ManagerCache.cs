using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers
{
    public class ManagerCache
    {
        public enum AddressType
        {
            STEALTH = 0,
            TRANSPARENT = 1
        }

        public class WalletAddress
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public AddressType Type { get; set; }
            public ulong Balance { get; set; }
            public bool Synced { get; set; }
            public bool Syncer { get; set; }
            public long Height { get; set; }

            public List<int> UTXOs { get; set; } = new List<int>();
        }

        public class WalletData
        {
            public string Label { get; set; }
            public ulong TotalBalance { get; set; }
            public long LastSeenHeight { get; set; }
            public bool Synced { get; set; }

            public List<WalletAddress> Addresses { get; set; } = new List<WalletAddress>();
        }

        public WalletData Wallet { get; set; }
    }
}
