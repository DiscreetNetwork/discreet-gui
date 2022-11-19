using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Discreet_GUI.Views.Layouts.Account
{
    public partial class AccountLeftNavigationLayoutView : ReactiveUserControl<AccountLeftNavigationLayoutViewModel>
    {
        public AccountLeftNavigationLayoutView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
