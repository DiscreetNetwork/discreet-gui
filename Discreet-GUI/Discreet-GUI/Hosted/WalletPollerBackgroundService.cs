using Microsoft.Extensions.Hosting;
using Services.Caches;
using Services.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Services.Daemon.Wallet;
using Services.Daemon.Status;

namespace Discreet_GUI.Hosted
{
    public class WalletPollerBackgroundService : BackgroundService
    {
        private readonly WalletCache _walletCache;
        private readonly DaemonWalletService _walletService;
        private readonly DaemonStatusService _statusService;

        public WalletPollerBackgroundService(WalletCache walletCache, DaemonWalletService walletService, DaemonStatusService statusService)
        {
            _walletCache = walletCache;
            _walletService = walletService;
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

                    _walletCache.EntropyHash = BitConverter.ToString(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(walletToFind.Entropy)));
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
            if (numberOfConnections is null) return;

            var previous = _walletCache.NumberOfConnections;
            if (numberOfConnections != previous) _walletCache.NumberOfConnections = numberOfConnections.Value;
        }

        
        public async Task UpdateAddressBalances()
        {
            foreach (var address in _walletCache.Accounts)
            {
                var fetchedBalance = await _walletService.GetBalance(address.Address);
                if(fetchedBalance == null)
                {
                    continue;
                }

                if (address.Balance != fetchedBalance) address.Balance = fetchedBalance.Value;
            }
        }


        public async Task UpdateAddressHeights()
        {
            foreach (var address in _walletCache.Accounts)
            {
                var addressState = await _walletService.GetAddressHeight(address.Address);
                if(addressState is null)
                {
                    continue;
                }

                if (address.Height != addressState.Height) address.Height = addressState.Height;
                if (address.Syncer != addressState.Syncer) address.Syncer = addressState.Syncer;
                if (address.Synced != addressState.Synced) address.Synced = addressState.Synced;
            }
        }

        public async Task UpdateWalletHeight()
        {
            var walletState = await _walletService.GetWalletHeight(_walletCache.Label);
            if(walletState is null)
            {
                return;
            }

            if (_walletCache.LastSeenHeight != walletState.Height) _walletCache.LastSeenHeight = walletState.Height;
            if (_walletCache.Synced != walletState.Synced) _walletCache.Synced = walletState.Synced;
        }
    }
}
