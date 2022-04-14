using Microsoft.Extensions.Hosting;
using Services.Daemon;
using Services.Daemon.Common;
using Services.Daemon.Models;
using Services.Daemon.Responses;
using Services.Daemon.Services;
using Services.Jazzicon;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WPF.Caches;
using WPF.ExtensionMethods;
using WPF.Services;

namespace WPF.Hosted
{
    public class WalletPollerBackgroundService : BackgroundService
    {
        private readonly WalletCache _walletCache;
        private readonly WalletService _walletService;
        private readonly NotificationService _notificationService;
        private readonly StatusService _statusService;

        public WalletPollerBackgroundService(WalletCache walletCache, WalletService walletService, NotificationService notificationService, StatusService statusService)
        {
            _walletCache = walletCache;
            _walletService = walletService;
            _notificationService = notificationService;
            _statusService = statusService;
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
                    var walletToFind = await _walletService.GetWallet(_walletCache.Label);
                    if (walletToFind == null) throw new Exception("WalletPollerBackgroundService: Could not find the selected wallet");

                    _walletCache.LastSeenHeight = walletToFind.LastSeenHeight;
                    _walletCache.Synced = walletToFind.Synced;

                    walletToFind.Addresses.ForEach(a =>
                    {
                        var accnt = new WalletCache.WalletAddress
                        {
                            Name = a.Name,
                            Address = a.Address,
                            Type = a.Type == 0 ? WalletCache.AddressType.STEALTH : WalletCache.AddressType.TRANSPARENT,
                            Balance = a.Balance,
                            Synced = a.Synced,
                            Syncer = a.Syncer,
                            UTXOs = new ObservableCollection<int>(a.UTXOs)
                        };

                        var icon = JazziconEx.IdenticonToAvaloniaBitmap(160, accnt.Address);
                        accnt.Identicon = icon;

                        _walletCache.Accounts.Add(accnt);
                    });

                    _walletCache.Initialized = true;
                }


                // poll the wallet
                if(_walletCache.Initialized)
                {
                    await UpdateAddressBalances();
                    await UpdateAddressHeights();
                    await UpdateWalletHeight();
                    await UpdatePeerCount();
                }
                

                await Task.Delay(100);
            }
        }


        public async Task UpdatePeerCount()
        {
            var numberOfConnections = await _statusService.GetNumConnections();
            if (numberOfConnections == -1) return;

            var previous = _walletCache.NumberOfConnections;
            if (numberOfConnections != previous) _walletCache.NumberOfConnections = numberOfConnections;
        }

        // FIX THIS TO USE WALLET SERVICE
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
                                if(address.Balance < balance)
                                {
                                    _notificationService.Display("You received some DIS!");
                                }
                                else
                                {
                                    _notificationService.Display("You successfully sent some DIS!");
                                }

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
