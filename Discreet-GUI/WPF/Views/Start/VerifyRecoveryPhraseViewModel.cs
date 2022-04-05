using Avalonia.Controls.Selection;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using WPF.Caches;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.Views.CreateWallet;
using WPF.Views.Layouts;

namespace WPF.Views.Start
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    public class VerifyRecoveryPhraseViewModel : ViewModelBase
    {
        public ObservableCollection<MnemonicWord> GeneratedWords { get; set; } = new ObservableCollection<MnemonicWord>();
        public ObservableCollection<MnemonicWord> RandomizedWords { get; set; } = new ObservableCollection<MnemonicWord>();
        public ObservableCollection<MnemonicWord> SelectedWords { get; set; } = new ObservableCollection<MnemonicWord>();
        public SelectionModel<MnemonicWord> Selection { get; } = new SelectionModel<MnemonicWord>();

        private string _continueButtonContent = "Not correct";
        public string ContinueButtonContent { get => _continueButtonContent; set { _continueButtonContent = value; OnPropertyChanged(nameof(ContinueButtonContent)); } }

        public ReactiveCommand<Unit, Unit> NavigateNextCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }

        public VerifyRecoveryPhraseViewModel() { }
        public VerifyRecoveryPhraseViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache)
        {
            NavigateNextCommand = ReactiveCommand.Create(navigationServiceFactory.Create<WalletPasswordViewModel>().Navigate);
            NavigateBackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<YourRecoveryPhraseViewModel>().Navigate);

            var random = new Random();
            newWalletCache.SeedPhrase.ForEach(p => GeneratedWords.Add(p));
            newWalletCache.SeedPhrase.OrderBy(x => random.Next(0, newWalletCache.SeedPhrase.Count)).ToList().ForEach(p => RandomizedWords.Add(p));

            Selection.SingleSelect = false;
            Selection.SelectionChanged += SelectionChanged;
        }

        void SelectionChanged(object sender, SelectionModelSelectionChangedEventArgs e)
        {
            foreach (var item in e.SelectedItems)
            {
                SelectedWords.Add(item as MnemonicWord);
            }

            foreach (var item in e.DeselectedItems)
            {
                SelectedWords.Remove(item as MnemonicWord);
            }

            if (GeneratedWords.SequenceEqual(SelectedWords))
                ContinueButtonContent = "Correct!";
            else
                ContinueButtonContent = "Not correct";
        }
    }
}
