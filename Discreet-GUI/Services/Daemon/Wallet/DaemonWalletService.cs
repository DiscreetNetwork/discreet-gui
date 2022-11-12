using Services.Daemon.Common;
using Services.Daemon.Wallet.Models;
using Services.Daemon.Wallet.Requests;
using Services.Daemon.Wallet.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Daemon.Wallet
{
    /// <summary>
    /// Used to interact with the Wallet API in the Daemon.
    /// </summary>
    public class DaemonWalletService
    {
        private RPCServer _rpcServer;

        public DaemonWalletService(RPCServer rpcServer)
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
        public async Task<Models.Wallet> CreateWallet(string label, string mnemonic, string passphrase)
        {
            var createWalletRequest = new Requests.CreateWalletRequest
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
        public async Task<Models.Wallet> CreateWallet(Requests.CreateWalletRequest @params)
        {
            var req = new DaemonRequest("create_wallet", @params);

            var resp = await _rpcServer.Request(req);
            if (resp is null) return null;

            if(resp.ContainsError(out var error))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<Models.Wallet>((JsonElement)resp.Result);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves a wallet based on the provided label
        /// </summary>
        /// <param name="label"></param>
        /// <returns>A <see cref="Wallet"/> on sucess; <see langword="null"/> if the call failed.</returns>
        public async Task<Models.Wallet> GetWallet(string label)
        {
            var wallets = await GetWallets();
            if (wallets is null) return null;

            var walletToFind = wallets.Where(w => w.Label.Equals(label)).FirstOrDefault();
            if (walletToFind is null) return null;

            return walletToFind;
        }

        /// <summary>
        /// Retrieves all wallets from the Daemon's WalletDB.
        /// </summary>
        /// <returns>A <see cref='List{T}'/> of <see cref='Wallet'/> on success; <see langword='null'/> if the call fails.</returns>
        public async Task<List<Models.Wallet>> GetWallets()
        {
            var req = new DaemonRequest("get_wallets_from_db");

            var resp = await _rpcServer.Request(req);
            if (resp is null) return null;

            if (resp.ContainsError(out var error))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<List<Models.Wallet>>((JsonElement)resp.Result);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves all wallet statuses from the Daemon, including unloaded wallets.
        /// </summary>
        /// <returns>A <see cref='List{T}'/> of <see cref='WalletStatusData'/> containing wallet status data on success; <see langword='null'/> if the call fails.</returns>
        public async Task<List<Models.WalletStatusData>> GetWalletStatuses()
        {
            var req = new DaemonRequest("get_wallet_statuses");

            var resp = await _rpcServer.Request(req);
            if (resp is null) return null;

            if (resp.ContainsError(out var error))
            {
                return null;
            }

            try
            {
                var data = JsonSerializer.Deserialize<List<Models.WalletStatusRV>>((JsonElement)resp.Result);
                return data.Select(x => new Models.WalletStatusData { Label = x.Label, Status = (Models.WalletStatus)x.Status }).ToList();
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
        public async Task<bool> LoadWallet(string label, string passphrase)
        {
            var loadWalletRequest = new Requests.LoadWalletRequest
            {
                Label = label,
                Passphrase = passphrase
            };

            var req = new DaemonRequest("load_wallet", loadWalletRequest);

            var resp = await _rpcServer.Request(req);
            if (resp is null) return false;

            if (resp.ContainsError(out var error))
            {
                return false;
            }

            return true;
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
            if (resp is null) return false;

            if (resp.ContainsError(out var error))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Decrypts the wallet in-memory and restores functionality.
        /// </summary>
        /// <param name="label">The label of the wallet to decrypt in the Daemon.</param>
        /// <param name="passphrase">The passphrase to the wallet to decrypt in the Daemon.</param>
        /// <returns><see langword='true'/> on success; <see langword='false'/> otherwise.</returns>
        public async Task<bool> UnlockWallet(string label, string passphrase)
        {
            var unlockWalletRequest = new Requests.UnlockWalletRequest
            {
                Label = label,
                Passphrase = passphrase
            };

            var req = new DaemonRequest("unlock_wallet", unlockWalletRequest);

            var resp = await _rpcServer.Request(req);
            if (resp is null) return false;

            if (resp.ContainsError(out var error))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates an address in an associated wallet with the specified parameters.
        /// </summary>
        /// <param name="label">The label of the wallet to create the address in.</param>
        /// <param name="name">The name of the address to create.</param>
        /// <param name="stealth">Whether or not the address to create is private.</param>
        /// <returns>The newly created <see cref='WalletAddress'/> on success; <see langword='null'/> if the call fails.</returns>
        public async Task<Models.WalletAddress> CreateAddress(string label, string name, bool stealth)
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
            if (resp is null) return null;

            if (resp.ContainsError(out var error))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<Models.WalletAddress>((JsonElement)resp.Result);
            }
            catch
            {
                return null;
            }
        }

        

        public async Task<Models.Wallet> RecoverWallet(string newLabel, string mnemonic, string passphrase)
        {
            return await CreateWallet(newLabel, mnemonic, "b");
        } 

        

        /// <summary>
        /// Returns the transaction history for the specified account's wallet address.
        /// <param name="address"></param>
        /// <returns></returns>
        public async Task<List<TransactionHistoryElement>> GetTransactionHistory(string address)
        {
            var req = new DaemonRequest("get_transaction_history", address);

            var resp = await _rpcServer.Request(req);
            if (resp is null) return null;

            if (resp.ContainsError(out var error))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<List<TransactionHistoryElement>>((JsonElement)resp.Result);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Searches the wallet for a specific transaction
        /// </summary>
        /// <param name="walletLabel"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task<TransactionHistoryElement> GetTransaction(string address, string transactionId)
        {
            var transactions = await GetTransactionHistory(address);

            if (transactions is null) return null;

            return transactions.Where(t => t.TxID == transactionId).FirstOrDefault();
        }

        public async Task<Mnemonic> GetMnemonic()
        {
            var req = new DaemonRequest("get_mnemonic");

            var resp = await _rpcServer.Request(req);
            if (resp is null) return null;

            if (resp.ContainsError(out var error))
            {
                return null;
            }

            try
            {
                var result = JsonSerializer.Deserialize<GetMnemonicResponse>((JsonElement)resp.Result);

                return new Mnemonic
                {
                    Value = result.Mnemonic,
                    Entropy = result.Entropy
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// State of the wallet
        /// </summary>
        /// <returns>Wallet height & synced status</returns>
        public async Task<GetWalletHeightResponse> GetWalletHeight(string label)
        {
            var req = new DaemonRequest("get_wallet_height", label);

            var resp = await _rpcServer.Request(req);
            if (resp is null) return null;

            if (resp.ContainsError(out var error))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<GetWalletHeightResponse>((JsonElement)resp.Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<GetAddressHeightResponse> GetAddressHeight(string accountAddress)
        {
            var req = new DaemonRequest("get_address_height", accountAddress);

            var resp = await _rpcServer.Request(req);
            if (resp is null) return null;

            if (resp.ContainsError(out var error))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<GetAddressHeightResponse>((JsonElement)resp.Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ulong?> GetBalance(string accountAddress)
        {
            var req = new DaemonRequest("get_balance", accountAddress);

            var resp = await _rpcServer.Request(req);
            if (resp is null) return null;

            if (resp.ContainsError(out var error))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<ulong>((JsonElement)resp.Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
