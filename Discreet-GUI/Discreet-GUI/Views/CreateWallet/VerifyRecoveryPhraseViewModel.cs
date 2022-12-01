using Avalonia.Controls.Selection;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using Services.Caches;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Layouts;
using System.Reactive.Disposables;

namespace Discreet_GUI.Views.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    public class VerifyRecoveryPhraseViewModel : ViewModelBase, IActivatableViewModel
    {
        public ObservableCollection<string> RandomizedMnemonic { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> SelectedWords { get; set; } = new ObservableCollection<string>();
        public SelectionModel<string> Selection { get; } = new SelectionModel<string>();

        private string _continueButtonContent = "Not correct";
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly NewWalletCache _newWalletCache;

        public string ContinueButtonContent { get => _continueButtonContent; set { _continueButtonContent = value; OnPropertyChanged(nameof(ContinueButtonContent)); } }

        public ViewModelActivator Activator { get; set; }
        public VerifyRecoveryPhraseViewModel() { }
        public VerifyRecoveryPhraseViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache)
        {
            Selection.SingleSelect = false;
            Selection.SelectionChanged += SelectionChanged;
            _navigationServiceFactory = navigationServiceFactory;
            _newWalletCache = newWalletCache;

            Activator = new ViewModelActivator();
            this.WhenActivated(d =>
            {
                var random = new Random();
                var randomizedMnemoic = _newWalletCache.Mnemonic.OrderBy(x => random.Next(0, _newWalletCache.Mnemonic.Count)).ToList();
                RandomizedMnemonic = new ObservableCollection<string>(randomizedMnemoic);
                OnPropertyChanged(nameof(RandomizedMnemonic));

                Disposable.Create(() => 
                {
                    Selection.SelectionChanged -= SelectionChanged;
                }).DisposeWith(d);
            });
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

        void NavigateNextCommand()
        {
            _navigationServiceFactory.Create<WalletPasswordViewModel>().Navigate();
        }
        
        void NavigateBackCommand()
        {
            _navigationServiceFactory.Create<YourRecoveryPhraseViewModel>().Navigate();
        }
    }
}
