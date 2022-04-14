using Services.Daemon;
using Services.Daemon.Services;
using Services.ZMQ.Handlers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ZMQ.Handlers
{
    public class BlockRawHandler : MessageHandler
    {
        private readonly WalletService _walletService;
        private readonly AccountService _accountService;

        public BlockRawHandler(WalletService walletService, AccountService accountService)
        {
            _walletService = walletService;
            _accountService = accountService;
        }

        public override void Handle(string message)
        {

        }
    }
}
