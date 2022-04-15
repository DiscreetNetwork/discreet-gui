using Avalonia.Input;
using Avalonia.Interactivity;
using Services.Caches;
using Services.Daemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Factories.Navigation;
using WPF.Services;
using WPF.ViewModels.Common;

namespace WPF.Views.Account.Modals
{
    public class CreateNewAccountViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly WalletService _walletService;
        private readonly NotificationService _notificationService;
        private readonly WalletCache _walletCache;

        public string AccountName { get; set; }
        public List<string> AccountTypes { get; set; } = new List<string> { "Stealth", "Transparent" };
        public int SelectedAccountTypeIndex { get; set; }


        public CreateNewAccountViewModel(NavigationServiceFactory navigationServiceFactory, WalletService walletService, NotificationService notificationService, WalletCache walletCache)
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
                _notificationService.Display("Failed to create account");
            }
            else
            {
                _notificationService.Display("Successfully created new account");
            }

            _walletCache.AddAccount(createdAddress.Name, createdAddress.Address, createdAddress.Balance, (WalletCache.AddressType)createdAddress.Type);
            Dismiss();
        }

        void Dismiss() => _navigationServiceFactory.CreateModalNavigationService().Navigate();
    }
}
