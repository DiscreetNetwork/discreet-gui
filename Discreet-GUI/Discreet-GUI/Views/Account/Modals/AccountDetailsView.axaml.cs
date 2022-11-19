using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Discreet_GUI.Views.Account.Modals
{
    public partial class AccountDetailsView : ReactiveUserControl<AccountDetailsViewModel>
    {
        public AccountDetailsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
