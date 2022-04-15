using Services.Caches;
using Services.Daemon;
using Services.Daemon.Services;
using Services.ZMQ.Handlers.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ZMQ.Handlers
{
    /// <summary>
    /// Whenever we receive a 'blockraw' message from the Daemon, we will fetch data for the wallet, to make sure the view is synced with the daemon
    /// </summary>
    public class BlockRawHandler : MessageHandler
    {
        private readonly WalletService _walletService;
        private readonly AccountService _accountService;
        private readonly WalletCache _walletCache;
        private readonly StatusService _statusService;

        public BlockRawHandler(WalletService walletService, AccountService accountService, WalletCache walletCache, StatusService statusService)
        {
            _walletService = walletService;
            _accountService = accountService;
            _walletCache = walletCache;
            _statusService = statusService;
        }

        public override void Handle(string message)
        {
            Debug.WriteLine("ZMQ.BlockRawHandler: Executing");
            Task[] tasks = new Task[] { UpdatePeerCount(), UpdateAddressBalances(), UpdateAddressHeights(), UpdateWalletHeight() };
            Task.WaitAll(tasks);
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
                if (fetchedBalance == null)
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
                if (addressState is null)
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
            if (walletState is null)
            {
                Debug.WriteLine($"WalletPollerBackgroundService: Failed to fetch state for wallet: {_walletCache.Label}");
                return;
            }

            if (_walletCache.LastSeenHeight != walletState.Height) _walletCache.LastSeenHeight = walletState.Height;
            if (_walletCache.Synced != walletState.Synced) _walletCache.Synced = walletState.Synced;
        }
    }
}
