using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Discreet_GUI.Views.Account
{
    public partial class AccountHomeView : ReactiveUserControl<AccountHomeViewModel>
    {
        public AccountHomeView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
