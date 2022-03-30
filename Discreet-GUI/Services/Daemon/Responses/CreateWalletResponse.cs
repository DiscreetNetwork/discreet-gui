using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Services.Daemon.Common;
using Services.Daemon.Requests;

namespace Services.Daemon.Responses
{
    public class WalletAddress
    {
        public string Name { get; set; }
        public byte Type { get; set; }

        public bool Deterministic { get; set; }

        public string EncryptedSecSpendKey { get; set; }
        public string EncryptedSecViewKey { get; set; }
        public string PubSpendKey { get; set; }
        public string PubViewKey { get; set; }
        public string Address { get; set; }

        public string EncryptedSecKey { get; set; }
        public string PubKey { get; set; }

        public bool Synced { get; set; }
        public bool Syncer { get; set; }
        public long LastSeenHeight { get; set; }

        public ulong Balance { get; set; }

        public List<int> UTXOs { get; set; }
    }

    public class Wallet : DaemonErrorResult
    {
        public string Label { get; set; }
        public string CoinName { get; set; }
        public bool Encrypted { get; set; }
        public ulong Timestamp { get; set; }
        public string Version { get; set; }
        public string Entropy { get; set; }
        public uint EntropyLen { get; set; }
        public ulong EntropyChecksum { get; set; }
        public long LastSeenHeight { get; set; }
        public bool Synced { get; set; }

        public List<WalletAddress> Addresses { get; set; }
    }

    public class CreateWalletResponse: Wallet
    {
        public CreateWalletResponse() { }

        public static CreateWalletResponse CreateWallet(RPCServer rpcServer, string label, string mnemonic, string passphrase)
        {
            var createWalletParams = new CreateWalletParams
            {
                Label = label,
                Mnemonic = mnemonic,
                Passphrase = passphrase,
                StealthAddressNames = new List<string>(new string[] { label + "Stealth" }),
                TransparentAddressNames = new List<string>(new string[] { label + "Transparent" }),
                Save = true,
                ScanForBalance = true
            };

            var req = new DaemonRequest("create_wallet", createWalletParams);

            var resp = rpcServer.RequestUnsafe(req);

            var result = JsonSerializer.Deserialize<CreateWalletResponse>((JsonElement)resp.Result);

            if (result.ErrMsg != null && result.ErrMsg != "")
            {
                System.Diagnostics.Debug.WriteLine("GetMnemonic : " + result.ErrMsg);
            }

            return result;
        }
    }
}
