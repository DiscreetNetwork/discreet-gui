using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WPF.Views.CreateWallet
{
    public partial class WalletCreatedSuccessfullyView : UserControl
    {
        public WalletCreatedSuccessfullyView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
