using Avalonia.Controls.Selection;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.ViewModels.CreateWallet;
using WPF.ViewModels.Layouts;

namespace WPF.ViewModels.Start
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    public class VerifyRecoveryPhraseViewModel : ViewModelBase
    {
        public ObservableCollection<MnemonicWord> GeneratedWords { get; set; } = new ObservableCollection<MnemonicWord>(MnemonicWord.GenerateWords(24));
        public ObservableCollection<MnemonicWord> SelectedWords { get; set; } = new ObservableCollection<MnemonicWord>();
        public SelectionModel<MnemonicWord> Selection { get; } = new SelectionModel<MnemonicWord>();

        private string _continueButtonContent = "Not correct";
        public string ContinueButtonContent { get => _continueButtonContent; set { _continueButtonContent = value; OnPropertyChanged(nameof(ContinueButtonContent)); } }

        public ReactiveCommand<Unit, Unit> NavigateNextCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }

        public VerifyRecoveryPhraseViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            NavigateNextCommand = ReactiveCommand.Create(navigationServiceFactory.Create<WalletPasswordViewModel>().Navigate);
            NavigateBackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<YourRecoveryPhraseViewModel>().Navigate);

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
