using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text;
using WPF.Stores.Navigation;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Layouts.Account
{
    [Layout(typeof(DarkTitleBarLayoutSimpleViewModel))]
    class AccountLeftNavigationLayoutViewModel : ViewModelBase
    {
        private readonly AccountNavigationStore _accountNavigationStore;
        public ViewModelBase CurrentViewModel => _accountNavigationStore.CurrentViewModel;

        public ReactiveCommand<Unit, Unit> NavigateAccountHomeCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateAccountSendCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateAccountReceiveCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateAccountTransactionsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateAccountSettingsCommand { get; set; }
        public ObservableCollection<bool> ButtonActiveStates { get; set; } = new ObservableCollection<bool>() { true, false, false, false, false };

        public AccountLeftNavigationLayoutViewModel(AccountNavigationStore accountNavigationStore)
        {
            _accountNavigationStore = accountNavigationStore;
            accountNavigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            NavigateAccountHomeCommand              = ReactiveCommand.Create(() => { ResetButtonStates(); ButtonActiveStates[0] = true; });
            NavigateAccountSendCommand              = ReactiveCommand.Create(() => { ResetButtonStates(); ButtonActiveStates[1] = true; });
            NavigateAccountReceiveCommand           = ReactiveCommand.Create(() => { ResetButtonStates(); ButtonActiveStates[2] = true; });
            NavigateAccountTransactionsCommand      = ReactiveCommand.Create(() => { ResetButtonStates(); ButtonActiveStates[3] = true; });
            NavigateAccountSettingsCommand          = ReactiveCommand.Create(() => { ResetButtonStates(); ButtonActiveStates[4] = true; });
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        void ResetButtonStates()
        {
            for (int i = 0; i < ButtonActiveStates.Count; i++)
            {
                ButtonActiveStates[i] = false;
            }
        }
    }
}
