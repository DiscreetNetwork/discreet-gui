using Services.Daemon.Common;
using Services.Daemon.SeedRecovery.Requests;
using Services.Daemon.SeedRecovery.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Daemon.SeedRecovery
{
    public class DaemonSeedRecoveryService
    {
        private readonly RPCServer _rpcServer;

        public DaemonSeedRecoveryService(RPCServer rpcServer)
        {
            _rpcServer = rpcServer;
        }

        /// <summary>
        /// Interacts with the Seed Recovery API to recover the mnemonic and seed of the specified wallet.
        /// </summary>
        /// <param name="label">Label of the wallet in the Daemon to get the mnemonic and seed for.</param>
        /// <param name="passphrase">Password to the wallet in the Daemon.</param>
        /// <returns>A <see cref='WalletRecoveryResponse'/> containing the mnemonic and seed to the wallet, on success; <see langword='null'/> otherwise.</returns>
        public async Task<GetWalletSeedResponse> RecoverWallet(GetWalletSeedRequest @params)
        {
            var req = new DaemonRequest("get_wallet_seed", @params.Label, @params.Passphrase);

            var resp = await _rpcServer.Request(req);
            if (resp is null) return null;

            if (resp.ContainsError(out var error))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<GetWalletSeedResponse>((JsonElement)resp.Result);
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
        public async Task<GetSecretKeyResponse> GetSecretKey(GetSecretKeyRequest @params)
        {
            var req = new DaemonRequest("get_secret_key", @params.Label, @params.Passphrase, @params.Address);

            var resp = await _rpcServer.Request(req);
            if (resp is null) return null;

            if (resp.ContainsError(out var error))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<GetSecretKeyResponse>((JsonElement)resp.Result);
            }
            catch
            {
                return null;
            }
        }
    }
}
