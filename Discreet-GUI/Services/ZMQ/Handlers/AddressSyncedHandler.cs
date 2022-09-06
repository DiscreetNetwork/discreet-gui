using Services.Caches;
using Services.ZMQ.Handlers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ZMQ.Handlers
{
    internal class AddressSyncedHandler : MessageHandler
    {
        private readonly WalletCache _walletCache;

        public AddressSyncedHandler(WalletCache walletCache)
        {
            _walletCache = walletCache;
        }

        public override async Task Handle(string address)
        {
            var accountToUpdate = _walletCache.Accounts.Where(a => a.Address.Equals(address)).FirstOrDefault();
            if (accountToUpdate is null || accountToUpdate.Synced == true) return;

            accountToUpdate.Synced = true;
        }
    }
}
