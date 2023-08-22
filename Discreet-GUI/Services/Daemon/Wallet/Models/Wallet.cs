using System.Collections.Generic;

namespace Services.Daemon.Wallet.Models
{
    public class Wallet
    {
        public string Path { get; set; }
        public string Label { get; set; }
        public string CoinName { get; set; }
        public bool Encrypted { get; set; }
        public ulong Timestamp { get; set; }
        public string Version { get; set; }
        public bool Locked { get; set; }
        //public string Entropy { get; set; }
        //public uint EntropyLen { get; set; }
        public string EntropyChecksum { get; set; }
        public long LastSeenHeight { get; set; }
        //public bool Synced { get; set; }

        //public List<WalletAddress> Addresses { get; set; }
        public List<string> Accounts { get; set; }
    }
}
