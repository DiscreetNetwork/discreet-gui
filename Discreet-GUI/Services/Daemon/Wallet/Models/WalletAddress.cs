using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.Wallet.Models
{
    public class WalletAddress
    {
        public string Name { get; set; }
        public byte Type { get; set; }

        public bool Deterministic { get; set; }

        //public string EncryptedSecSpendKey { get; set; }
        //public string EncryptedSecViewKey { get; set; }
        public string PubSpendKey { get; set; }
        public string PubViewKey { get; set; }
        public string Address { get; set; }

        //public string EncryptedSecKey { get; set; }
        public string PubKey { get; set; }

        //public bool Synced { get; set; }
        //public bool Syncer { get; set; }
        //public long LastSeenHeight { get; set; }

        public ulong Balance { get; set; }

        //public List<int> UTXOs { get; set; }
    }
}
