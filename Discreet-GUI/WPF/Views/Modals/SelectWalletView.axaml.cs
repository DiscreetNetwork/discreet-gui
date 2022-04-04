using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WPF.Views.Modals
{
    public partial class SelectWalletView : UserControl
    {
        public SelectWalletView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
