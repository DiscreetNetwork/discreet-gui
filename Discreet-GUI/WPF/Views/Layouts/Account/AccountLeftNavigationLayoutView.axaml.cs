using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WPF.Views.Layouts.Account
{
    public partial class AccountLeftNavigationLayoutView : UserControl
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
