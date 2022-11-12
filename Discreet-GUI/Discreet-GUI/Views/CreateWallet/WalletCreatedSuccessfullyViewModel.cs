using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Account;
using Discreet_GUI.Views.Layouts;
using Discreet_GUI.Views.Layouts.Account;

namespace Discreet_GUI.Views.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutSimpleViewModel))]
    class WalletCreatedSuccessfullyViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;

        public WalletCreatedSuccessfullyViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            _navigationServiceFactory = navigationServiceFactory;
        }


        void NavigateNextCommand()
        {
            _navigationServiceFactory.CreateAccountNavigation<AccountLeftNavigationLayoutViewModel>().Navigate();
            _navigationServiceFactory.CreateAccountNavigation<AccountHomeViewModel>().Navigate();
        }
    }
}
