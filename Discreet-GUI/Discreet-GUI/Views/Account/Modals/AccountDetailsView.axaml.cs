using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Discreet_GUI.Views.Account.Modals
{
    public partial class AccountDetailsView : UserControl
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
