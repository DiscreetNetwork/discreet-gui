﻿using Microsoft.Extensions.Hosting;
using Services.Daemon;
using Services.Daemon.Common;
using Services.Daemon.Responses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WPF.Caches;

namespace WPF.Hosted
{
    public class WalletPollerBackgroundService : BackgroundService
    {
        private readonly WalletCache _walletCache;
        private readonly RPCServer _rpcServer;

        public WalletPollerBackgroundService(WalletCache walletCache, RPCServer rpcServer)
        {
            _walletCache = walletCache;
            _rpcServer = rpcServer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                if (string.IsNullOrWhiteSpace(_walletCache.Label))
                {
                    await Task.Delay(100);
                    continue;
                }

                // first get the wallet
                if (!_walletCache.Initialized)
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

                                continue;
                            }

                            if (wallets.Count == 0)
                            {
                                System.Diagnostics.Debug.WriteLine("WalletManager getting wallets, count was zero");
                                continue;
                            }

                            var wallet = wallets.Where(x => x.Label == _walletCache.Label).FirstOrDefault();

                            if (wallet == null)
                            {
                                System.Diagnostics.Debug.WriteLine("WalletManager getting wallets, none with specified label");
                                continue;
                            }

                            _walletCache.Label = wallet.Label;
                            //_walletCache.TotalBalance = wallet.Addresses.Select(x => x.Balance).Aggregate((a, b) => a + b);
                            _walletCache.LastSeenHeight = wallet.LastSeenHeight;
                            _walletCache.Synced = wallet.Synced;

                            _walletCache.Accounts = new ExtensionMethods.ObservableCollectionEx<WalletCache.WalletAddress>(wallet.Addresses.Select(a => new WalletCache.WalletAddress
                            {
                                Name = a.Name,
                                Address = a.Address,
                                Type = a.Type == 0 ? WalletCache.AddressType.STEALTH : WalletCache.AddressType.TRANSPARENT,
                                Balance = a.Balance,
                                Synced = a.Synced,
                                Syncer = a.Syncer,
                                UTXOs = new ObservableCollection<int>(a.UTXOs)
                            }));

                            wallet.Addresses.ForEach(a => System.Diagnostics.Debug.WriteLine($"address is \"{a.Address}\""));

                            _walletCache.Initialized = true;
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("WalletManager getting wallets, result was null");
                    }
                }


                // poll the wallet
                if(_walletCache.Initialized)
                {
                    await UpdateAddressBalances();
                    await UpdateAddressHeights();
                    await UpdateWalletHeight();
                }
                

                await Task.Delay(100);
            }
        }




        public async Task UpdateAddressBalances()
        {
            foreach (var address in _walletCache.Accounts)
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

            //var totalBalance = _walletCache.Accounts.Select(x => x.Balance).Aggregate((a, b) => a + b);
            //if (_walletCache.TotalBalance != totalBalance) _walletCache.TotalBalance = totalBalance;
        }
        public async Task UpdateAddressHeights()
        {
            foreach (var address in _walletCache.Accounts)
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
                        }

                        if (address.Syncer != getAddressHeightResponse.Syncer)
                        {
                            address.Syncer = getAddressHeightResponse.Syncer;
                        }

                        if (address.Height != getAddressHeightResponse.Height)
                        {
                            address.Height = getAddressHeightResponse.Height;
                        }
                    }
                }
            }
        }
        public async Task UpdateWalletHeight()
        {
            var resp = await _rpcServer.Request(new DaemonRequest("get_wallet_height", _walletCache.Label));

            if (resp != null && resp.Result != null)
            {
                if (resp.Result is JsonElement json)
                {
                    var getWalletHeightResponse = JsonSerializer.Deserialize<GetWalletHeightResponse>(json);

                    if (getWalletHeightResponse.ErrMsg != null && getWalletHeightResponse.ErrMsg != "")
                    {
                        System.Diagnostics.Debug.WriteLine("WalletManager getting wallet height : " + getWalletHeightResponse.ErrMsg);
                    }

                    if (_walletCache.Synced != getWalletHeightResponse.Synced)
                    {
                        _walletCache.Synced = getWalletHeightResponse.Synced;
                    }

                    if (_walletCache.LastSeenHeight != getWalletHeightResponse.Height)
                    {
                        _walletCache.LastSeenHeight = getWalletHeightResponse.Height;
                    }
                }
            }
        }
    }
}
