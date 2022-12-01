using Services.Caches;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Services;
using Discreet_GUI.ViewModels.Common;
using Services.Daemon.Wallet;

namespace Discreet_GUI.Views.Account.Modals
{
    public class CreateNewAccountViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly DaemonWalletService _walletService;
        private readonly NotificationService _notificationService;
        private readonly WalletCache _walletCache;

        public string AccountName { get; set; }
        public List<string> AccountTypes { get; set; } = new List<string> { "Stealth", "Transparent" };
        public int SelectedAccountTypeIndex { get; set; }


        public CreateNewAccountViewModel(NavigationServiceFactory navigationServiceFactory, DaemonWalletService walletService, NotificationService notificationService, WalletCache walletCache)
        {
            _navigationServiceFactory = navigationServiceFactory;
            _walletService = walletService;
            _notificationService = notificationService;
            _walletCache = walletCache;
        }



        async Task Create()
        {
            string label = _walletCache.Label;
            bool stealth = AccountTypes[SelectedAccountTypeIndex].Equals("Stealth");

            var createdAddress = await _walletService.CreateAddress(label, AccountName, stealth);
            if(createdAddress is null)
            {
                _notificationService.DisplayError("An error occured while trying to create the account.");
            }
            else
            {
                _notificationService.DisplaySuccess("Successfully created the new account.");
            }

            _walletCache.AddAccount(createdAddress.Name, createdAddress.Address, createdAddress.Balance, (WalletCache.AddressType)createdAddress.Type);
            Dismiss();
        }

        void Dismiss() => _navigationServiceFactory.CreateModalNavigationService().Navigate();
    }
}
