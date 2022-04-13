using Avalonia.Controls.Selection;
using ReactiveUI;
using Services.Daemon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using WPF.Caches;
using WPF.Factories.Navigation;
using WPF.Services;
using WPF.ViewModels.Common;
using WPF.Views.CreateWallet;
using WPF.Views.Layouts;

namespace WPF.Views.Start
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    public class VerifyRecoveryPhraseViewModel : ViewModelBase
    {
        public ObservableCollection<string> RandomizedMnemonic { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> SelectedWords { get; set; } = new ObservableCollection<string>();
        public SelectionModel<string> Selection { get; } = new SelectionModel<string>();

        private string _continueButtonContent = "Not correct";
        private readonly NewWalletCache _newWalletCache;

        public string ContinueButtonContent { get => _continueButtonContent; set { _continueButtonContent = value; OnPropertyChanged(nameof(ContinueButtonContent)); } }

        public ReactiveCommand<Unit, Unit> NavigateNextCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }

        public VerifyRecoveryPhraseViewModel() { }
        public VerifyRecoveryPhraseViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache)
        {
            NavigateNextCommand = ReactiveCommand.Create(navigationServiceFactory.Create<WalletPasswordViewModel>().Navigate);
            NavigateBackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<YourRecoveryPhraseViewModel>().Navigate);

            Selection.SingleSelect = false;
            Selection.SelectionChanged += SelectionChanged;
            _newWalletCache = newWalletCache;

            RxApp.MainThreadScheduler.Schedule(OnActivated);
        }

        void OnActivated()
        {
            var random = new Random();
            var randomizedMnemoic = _newWalletCache.Mnemonic.OrderBy(x => random.Next(0, _newWalletCache.Mnemonic.Count)).ToList();
            RandomizedMnemonic = new ObservableCollection<string>(randomizedMnemoic);
            OnPropertyChanged(nameof(RandomizedMnemonic));
        }

        void SelectionChanged(object sender, SelectionModelSelectionChangedEventArgs e)
        {
            foreach (var item in e.SelectedItems)
            {
                SelectedWords.Add(item as string);
            }

            foreach (var item in e.DeselectedItems)
            {
                SelectedWords.Remove(item as string);
            }

            if (_newWalletCache.Mnemonic.SequenceEqual(SelectedWords))
                ContinueButtonContent = "Correct!";
            else
                ContinueButtonContent = "Not correct";
        }
    }
}
