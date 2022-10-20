using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Discreet_GUI.Views.CreateWallet
{
    public partial class WalletNameView : UserControl
    {
        public WalletNameView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
