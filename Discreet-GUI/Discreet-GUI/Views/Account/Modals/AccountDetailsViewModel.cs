using ReactiveUI;
using Services.Caches;
using Services.Extensions;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using System.Linq;
using static Services.Caches.WalletCache;

namespace Discreet_GUI.Views.Account.Modals
{
    public class AccountDetailsViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly WalletCache _walletCache;

        Avalonia.Media.Imaging.Bitmap _qrCode = null;
        public Avalonia.Media.Imaging.Bitmap QrCode { get => _qrCode; set { _qrCode = value; OnPropertyChanged(nameof(QrCode)); } }

        private WalletAddress _walletAddres;
        public WalletAddress WalletAddress { get => _walletAddres; set { _walletAddres = value; OnPropertyChanged(nameof(WalletAddress)); } }

        public AccountDetailsViewModel(NavigationServiceFactory navigationServiceFactory, WalletCache walletCache)
        {
            _navigationServiceFactory = navigationServiceFactory;
            _walletCache = walletCache;
            RxApp.MainThreadScheduler.Schedule(OnActivated);
        }

        void OnActivated()
        {
            QrCode = BitmapEx.CreateQRCode(_walletCache.SelectedAccount);
            WalletAddress = _walletCache.Accounts.Where(a => a.Address == _walletCache.SelectedAccount).FirstOrDefault();
        }

        async Task CopyQrCode()
        {
            
        }

        void Dismiss() => _navigationServiceFactory.CreateModalNavigationService().Navigate();
    }
}
