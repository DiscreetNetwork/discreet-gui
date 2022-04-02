using Services.Daemon.Common;
using Services.Daemon.Models;
using Services.Daemon.Requests;
using Services.Daemon.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Daemon
{
    /// <summary>
    /// Used to interact with the Wallet API in the Daemon.
    /// </summary>
    public class WalletService
    {
        private RPCServer _rpcServer;

        public WalletService(RPCServer rpcServer)
        {
            _rpcServer = rpcServer;
        }

        /// <summary>
        /// Creates a wallet in the Daemon with the specified parameters.
        /// </summary>
        /// <param name="label">The label of the wallet to create.</param>
        /// <param name="mnemonic">The mnemonic/seed phrase to use as the wallet entropy.</param>
        /// <param name="passphrase">The passphrase to encrypt the wallet with.</param>
        /// <returns>A <see cref='Wallet'/> on success; <see langword='null'/> if the call fails.</returns>
        public async Task<Wallet> CreateWallet(string label, string mnemonic, string passphrase)
        {
            var createWalletRequest = new CreateWalletRequest
            {
                Label = label,
                Mnemonic = mnemonic,
                Passphrase = passphrase,
                StealthAddressNames = new List<string>(new string[] { "Stealth" }),
                TransparentAddressNames = new List<string>(new string[] { "Transparent" }),
                Save = true,
                ScanForBalance = true
            };

            return await CreateWallet(createWalletRequest);
        }

        /// <summary>
        /// Creates a wallet in the Daemon with the specified parameters.
        /// </summary>
        /// <param name="params">The <see cref='CreateWalletRequest'/> to use when creating the wallet in the Daemon.</param>
        /// <returns>A <see cref='Wallet'/> on success; <see langword='null'/> if the call fails.</returns>
        public async Task<Wallet> CreateWallet(CreateWalletRequest @params)
        {
            var req = new DaemonRequest("create_wallet", @params);

            var resp = await _rpcServer.Request(req);

            try
            {
                return JsonSerializer.Deserialize<Wallet>((JsonElement)resp.Result);
            }
            catch
            {
                return null;
            }
        }

        public Wallet CreateWalletSync(string label, string mnemonic, string passphrase)
        {
            var createWalletRequest = new CreateWalletRequest
            {
                Label = label,
                Mnemonic = mnemonic,
                Passphrase = passphrase,
                StealthAddressNames = new List<string>(new string[] { "Stealth" }),
                TransparentAddressNames = new List<string>(new string[] { "Transparent" }),
                Save = true,
                ScanForBalance = true
            };

            return CreateWalletSync(createWalletRequest);
        }

        public Wallet CreateWalletSync(CreateWalletRequest @params)
        {
            var req = new DaemonRequest("create_wallet", @params);

            var resp = _rpcServer.RequestUnsafe(req);

            try
            {
                return JsonSerializer.Deserialize<Wallet>((JsonElement)resp.Result);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves all wallets from the Daemon's WalletDB.
        /// </summary>
        /// <returns>A <see cref='List{T}'/> of <see cref='Wallet'/> on success; <see langword='null'/> if the call fails.</returns>
        public async Task<List<Wallet>> GetWallets()
        {
            var req = new DaemonRequest("get_wallets_from_db");

            var resp = await _rpcServer.Request(req);

            try
            {
                return JsonSerializer.Deserialize<List<Wallet>>((JsonElement)resp.Result);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Loads a wallet from the Daemon's WalletDB with the specified parameters.
        /// </summary>
        /// <param name="label">The label of the wallet to load.</param>
        /// <param name="passphrase">The passphrase of the wallet to load.</param>
        /// <returns>The loaded <see cref='Wallet'/> on success; <see langword='null'/> if the call fails.</returns>
        public async Task<Wallet> LoadWallet(string label, string passphrase)
        {
            var loadWalletRequest = new LoadWalletRequest
            {
                Label = label,
                Passphrase = passphrase
            };

            var req = new DaemonRequest("load_wallet", loadWalletRequest);

            var resp = await _rpcServer.Request(req);

            try
            {
                return JsonSerializer.Deserialize<Wallet>((JsonElement)resp.Result);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Encrypts the wallet in-memory and prevents functionality while encrypted.
        /// </summary>
        /// <param name="label">The label of the wallet to encrypt in the Daemon.</param>
        /// <returns><see langword='true'/> on success; <see langword='false'/> otherwise.</returns>
        public async Task<bool> LockWallet(string label)
        {
            var req = new DaemonRequest("lock_wallet", label);

            var resp = await _rpcServer.Request(req);

            return resp.IsOK();
        }

        /// <summary>
        /// Decrypts the wallet in-memory and restores functionality.
        /// </summary>
        /// <param name="label">The label of the wallet to decrypt in the Daemon.</param>
        /// <param name="passphrase">The passphrase to the wallet to decrypt in the Daemon.</param>
        /// <returns><see langword='true'/> on success; <see langword='false'/> otherwise.</returns>
        public async Task<bool> UnlockWallet(string label, string passphrase)
        {
            var unlockWalletRequest = new UnlockWalletRequest
            {
                Label = label,
                Passphrase = passphrase
            };

            var req = new DaemonRequest("unlock_wallet", unlockWalletRequest);

            var resp = await _rpcServer.Request(req);

            return resp.IsOK();
        }

        /// <summary>
        /// Creates an address in an associated wallet with the specified parameters.
        /// </summary>
        /// <param name="label">The label of the wallet to create the address in.</param>
        /// <param name="name">The name of the address to create.</param>
        /// <param name="stealth">Whether or not the address to create is private.</param>
        /// <returns>The newly created <see cref='WalletAddress'/> on success; <see langword='null'/> if the call fails.</returns>
        public async Task<WalletAddress> CreateAddress(string label, string name, bool stealth)
        {
            var createAddressRequest = new CreateAddressRequest
            {
                Label = label,
                Type = stealth ? "private" : "public",
                Deterministic = true,
                Name = name,
                ScanForBalance = true
            };

            var req = new DaemonRequest("create_address", createAddressRequest);

            var resp = await _rpcServer.Request(req);

            try
            {
                return JsonSerializer.Deserialize<WalletAddress>((JsonElement)resp.Result);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Interacts with the Seed Recovery API to recover the mnemonic and seed of the specified wallet.
        /// </summary>
        /// <param name="label">Label of the wallet in the Daemon to get the mnemonic and seed for.</param>
        /// <param name="passphrase">Password to the wallet in the Daemon.</param>
        /// <returns>A <see cref='WalletRecoveryResponse'/> containing the mnemonic and seed to the wallet, on success; <see langword='null'/> otherwise.</returns>
        public async Task<WalletRecoveryResponse> RecoverWallet(string label, string passphrase)
        {
            var req = new DaemonRequest("get_wallet_seed", label, passphrase);

            var resp = await _rpcServer.Request(req);

            try
            {
                return JsonSerializer.Deserialize<WalletRecoveryResponse>((JsonElement)resp.Result);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Interacts with the Seed Recovery API to recover the secret key(s) of the specified wallet's address.
        /// </summary>
        /// <param name="label">Label of the wallet in the Daemon the address is in.</param>
        /// <param name="passphrase">Password to the wallet in the Daemon.</param>
        /// <param name="address">Address for the wallet in the Daemon to recover the secret key(s) for.</param>
        /// <returns>A <see cref='AddressRecoveryResponse'/> containing the mnemonic and hex seed of the secret key(s) to the wallet address, on success; <see langword='null'/> otherwise.</returns>
        public async Task<AddressRecoveryResponse> RecoverAddress(string label, string passphrase, string address)
        {
            var req = new DaemonRequest("get_secret_key", label, passphrase, address);

            var resp = await _rpcServer.Request(req);

            try
            {
                return JsonSerializer.Deserialize<AddressRecoveryResponse>((JsonElement)resp.Result);
            }
            catch
            {
                return null;
            }
        }
    }
}
