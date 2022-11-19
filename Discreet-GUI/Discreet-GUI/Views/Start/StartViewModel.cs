using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.CreateWallet;
using Discreet_GUI.Views.Layouts;
using System.Reactive.Concurrency;
using System.Linq;
using Discreet_GUI.Views.Modals;
using Services.Daemon.Wallet;
using Discreet_GUI.Services;
using System.Reactive.Disposables;

namespace Discreet_GUI.Views.Start
{
    [Layout(typeof(PurpleTitleBarLayoutViewModel))]
    public class StartViewModel : ViewModelBase, IActivatableViewModel
    {
        /// <summary>
        /// Determines what Image to display in the carousel
        /// </summary>
        private int _carouselIndex = 0;
        public int CarouselIndex { get => _carouselIndex; set { _carouselIndex = value; OnPropertyChanged(nameof(CarouselIndex)); } }

        /// <summary>
        /// Controls what class will be active in the process dots image
        /// </summary>
        public ObservableCollection<bool> Steps { get; set; } = new ObservableCollection<bool>
        {
            true,
            false,
            false
        };

        /// <summary>
        /// Controls the carousel loop
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly NavigationServiceFactory _navigationServiceFactory;

        public string CurrentVersion { get => $"Version: {Assembly.GetExecutingAssembly().GetName().Version.Major}.{Assembly.GetExecutingAssembly().GetName().Version.Minor}.{Assembly.GetExecutingAssembly().GetName().Version.Build}"; }

        public ViewModelActivator Activator { get; set; }

        public StartViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            Activator = new ViewModelActivator();
            this.WhenActivated(d =>
            {
                _ = InitializeCarousel(_cancellationTokenSource.Token);

                Disposable.Create(() => _cancellationTokenSource.Cancel()).DisposeWith(d);
            });

            
            _navigationServiceFactory = navigationServiceFactory;
        }

        void NavigateCreateWalletChoicesViewCommand()
        {
            _cancellationTokenSource.Cancel();
            _navigationServiceFactory.Create<WalletNameViewModel>().Navigate();
        }

        void NavigateExistingWalletChoicesViewCommand()
        {
            _navigationServiceFactory.Create<ExistingWalletChoicesViewModel>().Navigate();
        }

        async Task InitializeCarousel(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(2000);
                ResetSteps();

                if (CarouselIndex == 0)
                {
                    CarouselIndex = 1;
                    Steps[1] = true;
                }

                else if(CarouselIndex == 1)
                {
                    CarouselIndex = 2;
                    Steps[2] = true;
                }

                else if (CarouselIndex == 2)
                {
                    CarouselIndex = 0;
                    Steps[0] = true;
                }
            }
        }

        

        void ResetSteps()
        {
            for(var i = 0; i < Steps.Count; i++)
            {
                Steps[i] = false;
            }
        }
    }
}
