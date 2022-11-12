using Services.Caches;
using Services.Daemon.Status;
using Services.Daemon.Wallet;
using Services.ZMQ.Handlers.Common;
using System.Threading.Tasks;

namespace Services.ZMQ.Handlers
{
    /// <summary>
    /// This handler runs whenever the daemon published the 'processblocknotify' message
    /// which happens if the processed block changed the internal state of the daemon
    /// </summary>
    public class ProcessBlockNotifyHandler : MessageHandler
    {
        private readonly DaemonWalletService _walletService;
        private readonly WalletCache _walletCache;
        private readonly DaemonStatusService _statusService;

        public ProcessBlockNotifyHandler(DaemonWalletService walletService, WalletCache walletCache, DaemonStatusService statusService)
        {
            _walletService = walletService;
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
            if (numberOfConnections is null) return;

            var previous = _walletCache.NumberOfConnections;
            if (numberOfConnections != previous) _walletCache.NumberOfConnections = numberOfConnections.Value;
        }


        public async Task UpdateAddressBalances()
        {
            foreach (var address in _walletCache.Accounts)
            {
                var fetchedBalance = await _walletService.GetBalance(address.Address);
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
                var addressState = await _walletService.GetAddressHeight(address.Address);
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
            var walletState = await _walletService.GetWalletHeight(_walletCache.Label);
            if (walletState is null)
            {
                return;
            }

            if (_walletCache.LastSeenHeight != walletState.Height) _walletCache.LastSeenHeight = walletState.Height;
            if (_walletCache.Synced != walletState.Synced) _walletCache.Synced = walletState.Synced;
        }
    }
}
