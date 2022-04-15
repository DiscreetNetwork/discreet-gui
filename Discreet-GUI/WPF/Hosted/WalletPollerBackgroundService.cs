using Microsoft.Extensions.Hosting;
using Services.Caches;
using Services.Daemon;
using Services.Daemon.Common;
using Services.Daemon.Models;
using Services.Daemon.Responses;
using Services.Daemon.Services;
using Services.Extensions;
using Services.Jazzicon;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WPF.Services;

namespace WPF.Hosted
{
    public class WalletPollerBackgroundService : BackgroundService
    {
        private readonly WalletCache _walletCache;
        private readonly WalletService _walletService;
        private readonly NotificationService _notificationService;
        private readonly StatusService _statusService;
        private readonly AccountService _accountService;

        public WalletPollerBackgroundService(WalletCache walletCache, WalletService walletService, NotificationService notificationService, StatusService statusService, AccountService accountService)
        {
            _walletCache = walletCache;
            _walletService = walletService;
            _notificationService = notificationService;
            _statusService = statusService;
            _accountService = accountService;
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

                    await UpdateAddressBalances();
                    await UpdateAddressHeights();
                    await UpdateWalletHeight();
                    await UpdatePeerCount();

                    _walletCache.Initialized = true;
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

        
        public async Task UpdateAddressBalances()
        {
            foreach (var address in _walletCache.Accounts)
            {
                var fetchedBalance = await _accountService.GetBalance(address.Address);
                if(fetchedBalance == null)
                {
                    Debug.WriteLine($"WalletPollerBackgroundService: Failed to fetch balance for account: {address.Address}");
                    continue;
                }

                if (address.Balance != fetchedBalance) address.Balance = fetchedBalance.Value;
            }
        }


        public async Task UpdateAddressHeights()
        {
            foreach (var address in _walletCache.Accounts)
            {
                var addressState = await _accountService.GetState(address.Address);
                if(addressState is null)
                {
                    Debug.WriteLine($"WalletPollerBackgroundService: Failed to fetch state for account: {address.Address}");
                    continue;
                }

                if (address.Height != addressState.Height) address.Height = addressState.Height;
                if (address.Syncer != addressState.Syncer) address.Syncer = addressState.Syncer;
                if (address.Synced != addressState.Synced) address.Synced = addressState.Synced;
            }
        }

        public async Task UpdateWalletHeight()
        {
            var walletState = await _walletService.GetState(_walletCache.Label);
            if(walletState is null)
            {
                Debug.WriteLine($"WalletPollerBackgroundService: Failed to fetch state for wallet: {_walletCache.Label}");
                return;
            }

            if (_walletCache.LastSeenHeight != walletState.Height) _walletCache.LastSeenHeight = walletState.Height;
            if (_walletCache.Synced != walletState.Synced) _walletCache.Synced = walletState.Synced;
        }
    }
}
