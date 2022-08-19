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
    /// This handler runs whenever the daemon published the 'processblocknotify' message
    /// which happens if the processed block changed the internal state of the daemon
    /// </summary>
    public class ProcessBlockNotifyHandler : MessageHandler
    {
        private readonly WalletService _walletService;
        private readonly AccountService _accountService;
        private readonly WalletCache _walletCache;
        private readonly StatusService _statusService;

        public ProcessBlockNotifyHandler(WalletService walletService, AccountService accountService, WalletCache walletCache, StatusService statusService)
        {
            _walletService = walletService;
            _accountService = accountService;
            _walletCache = walletCache;
            _statusService = statusService;
        }

        public override async Task Handle(string message)
        {
            await UpdatePeerCount();
            await UpdateAddressBalances();
            await UpdateAddressHeights();
            await UpdateWalletHeight();
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
                return;
            }

            if (_walletCache.LastSeenHeight != walletState.Height) _walletCache.LastSeenHeight = walletState.Height;
            if (_walletCache.Synced != walletState.Synced) _walletCache.Synced = walletState.Synced;
        }
    }
}
