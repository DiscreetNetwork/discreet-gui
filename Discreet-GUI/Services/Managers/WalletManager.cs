using Services.Daemon;
using Services.Daemon.Common;
using Services.Daemon.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Managers
{
    public class WalletChangeEventArgs: EventArgs
    {
        public ManagerCache cache { get; set; }
    }

    public delegate void WalletChangeEventHandler(object sender, WalletChangeEventArgs e);

    public class WalletManager
    {
        private RPCServer _rpcServer;
        private ManagerCache _managerCache;

        public event WalletChangeEventHandler OnWalletChange;  

        public WalletManager(RPCServer rpcServer)
        {
            _rpcServer = rpcServer;
            _managerCache = new ManagerCache();
        }

        public async Task<bool> UpdateAddressBalances()
        {
            bool changed = false;
            foreach (var address in _managerCache.Wallet.Addresses)
            {
                var resp = await _rpcServer.Request(new DaemonRequest("get_balance", address.Address));

                if (resp != null && resp.Result != null)
                {
                    if (resp.Result is JsonElement json)
                    {
                        if (json.ValueKind == JsonValueKind.Number)
                        {
                            // the request was good
                            var balance = json.GetUInt64();

                            if (address.Balance != balance)
                            {
                                address.Balance = balance;
                                changed = true;
                            }
                        }
                        else
                        {
                            // error type
                            var err = JsonSerializer.Deserialize<DaemonErrorResult>(json);

                            System.Diagnostics.Debug.WriteLine("WalletManager getting balance : " + err.ErrMsg);
                        }
                    }
                }
            }

            _managerCache.Wallet.TotalBalance = _managerCache.Wallet.Addresses.Select(x => x.Balance).Aggregate((a, b) => a + b);

            return changed;
        }

        public async Task<bool> UpdateAddressHeights()
        {
            bool changed = false;
            foreach (var address in _managerCache.Wallet.Addresses)
            {
                var resp = await _rpcServer.Request(new DaemonRequest("get_address_height", address.Address));

                if (resp != null && resp.Result != null)
                {
                    if (resp.Result is JsonElement json)
                    {
                        var getAddressHeightResponse = JsonSerializer.Deserialize<GetAddressHeightResponse>(json);

                        if (getAddressHeightResponse.ErrMsg != null && getAddressHeightResponse.ErrMsg != "")
                        {
                            System.Diagnostics.Debug.WriteLine("WalletManager getting address height : " + getAddressHeightResponse.ErrMsg);
                        }

                        if (address.Synced != getAddressHeightResponse.Synced)
                        {
                            address.Synced = getAddressHeightResponse.Synced;
                            changed = true;
                        }

                        if (address.Syncer != getAddressHeightResponse.Syncer)
                        {
                            address.Syncer = getAddressHeightResponse.Syncer;
                            changed = true;
                        }

                        if (address.Height != getAddressHeightResponse.Height)
                        {
                            address.Height = getAddressHeightResponse.Height;
                            changed = true;
                        }
                    }
                }
            }

            return changed;
        }

        public async Task<bool> UpdateWalletHeight()
        {
            bool changed = false;
            var resp = await _rpcServer.Request(new DaemonRequest("get_wallet_height", _managerCache.Wallet.Label));

            if (resp != null && resp.Result != null)
            {
                if (resp.Result is JsonElement json)
                {
                    var getWalletHeightResponse = JsonSerializer.Deserialize<GetWalletHeightResponse>(json);

                    if (getWalletHeightResponse.ErrMsg != null && getWalletHeightResponse.ErrMsg != "")
                    {
                        System.Diagnostics.Debug.WriteLine("WalletManager getting wallet height : " + getWalletHeightResponse.ErrMsg);
                    }

                    if (_managerCache.Wallet.Synced != getWalletHeightResponse.Synced)
                    {
                        _managerCache.Wallet.Synced = getWalletHeightResponse.Synced;
                        changed = true;
                    }

                    if (_managerCache.Wallet.LastSeenHeight != getWalletHeightResponse.Height)
                    {
                        _managerCache.Wallet.LastSeenHeight = getWalletHeightResponse.Height;
                        changed = true;
                    }
                }
            }

            return changed;
        }

        public async Task Start(string label, CancellationToken token = default)
        {
            // first get the wallet
            if (_managerCache.Wallet == null)
            {
                var resp = await _rpcServer.Request(new DaemonRequest("get_wallets_from_db"));

                if (resp != null && resp.Result != null)
                {
                    if (resp.Result is JsonElement json)
                    {
                        List<Wallet> wallets;

                        // try to decode the result as a list of Wallets (defined in CreateWalletResponse.cs)
                        try
                        {
                            wallets = JsonSerializer.Deserialize<List<Wallet>>(json);
                        }
                        catch (Exception)
                        {
                            try
                            {
                                // first check if the call to the daemon returned an RPC error
                                DaemonErrorResult result = JsonSerializer.Deserialize<DaemonErrorResult>(json);

                                System.Diagnostics.Debug.WriteLine("WalletManager getting wallets : " + result.ErrMsg);
                            }
                            catch (Exception ex2)
                            {
                                System.Diagnostics.Debug.WriteLine("WalletManager getting wallets : " + ex2.Message);
                            }

                            return;
                        }

                        if (wallets.Count == 0)
                        {
                            System.Diagnostics.Debug.WriteLine("WalletManager getting wallets, count was zero");
                            return;
                        }

                        var wallet = wallets.Where(x => x.Label == label).FirstOrDefault();

                        if (wallet == null)
                        {
                            System.Diagnostics.Debug.WriteLine("WalletManager getting wallets, none with specified label");
                            return;
                        }

                        _managerCache.Wallet = new ManagerCache.WalletData
                        {
                            Label = wallet.Label,
                            TotalBalance = wallet.Addresses.Select(x => x.Balance).Aggregate((a, b) => a + b),
                            LastSeenHeight = wallet.LastSeenHeight,
                            Synced = wallet.Synced,
                            Addresses = new List<ManagerCache.WalletAddress>()
                        };

                        foreach (var addr in wallet.Addresses)
                        {
                            System.Diagnostics.Debug.WriteLine($"address is \"{addr.Address}\"");
                            _managerCache.Wallet.Addresses.Add(new ManagerCache.WalletAddress
                            {
                                Name = addr.Name,
                                Address = addr.Address,
                                Type = addr.Type == 0 ? ManagerCache.AddressType.STEALTH : ManagerCache.AddressType.TRANSPARENT,
                                Balance = addr.Balance,
                                Synced = addr.Synced,
                                Syncer = addr.Syncer,
                                UTXOs = addr.UTXOs
                            });
                        }
                    }   
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("WalletManager getting wallets, result was null");
                }
            }

            // poll
            while (!token.IsCancellationRequested)
            {
                var changed = false;
                changed = await UpdateAddressBalances() || changed;
                changed = await UpdateAddressHeights() || changed;
                changed = await UpdateWalletHeight() || changed;

                if (changed)
                {
                    OnWalletChange?.Invoke(this, new WalletChangeEventArgs { cache = _managerCache });
                }

                await Task.Delay(100);
            }
        }
    }
}
