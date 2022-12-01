using ReactiveUI;
using Services.Caches;
using Services.Extensions;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using System.Linq;
using static Services.Caches.WalletCache;
using System.Reactive.Disposables;

namespace Discreet_GUI.Views.Account.Modals
{
    public class AccountDetailsViewModel : ViewModelBase, IActivatableViewModel
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly WalletCache _walletCache;

        Avalonia.Media.Imaging.Bitmap _qrCode = null;
        public Avalonia.Media.Imaging.Bitmap QrCode { get => _qrCode; set { _qrCode = value; OnPropertyChanged(nameof(QrCode)); } }

        private WalletAddress _walletAddres;
        public WalletAddress WalletAddress { get => _walletAddres; set { _walletAddres = value; OnPropertyChanged(nameof(WalletAddress)); } }

        public ViewModelActivator Activator { get; set; }
        public AccountDetailsViewModel(NavigationServiceFactory navigationServiceFactory, WalletCache walletCache)
        {
            _navigationServiceFactory = navigationServiceFactory;
            _walletCache = walletCache;

            Activator = new ViewModelActivator();
            this.WhenActivated(d =>
            {
                QrCode = BitmapEx.CreateQRCode(_walletCache.SelectedAccount);
                WalletAddress = _walletCache.Accounts.Where(a => a.Address == _walletCache.SelectedAccount).FirstOrDefault();

                Disposable.Create(() => { }).DisposeWith(d);
            });
        }

        void Dismiss() => _navigationServiceFactory.CreateModalNavigationService().Navigate();
    }
}
