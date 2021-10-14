using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WPF.Views.Start
{
    public partial class ExistingWalletBootstrapChoicesView : UserControl
    {
        public ExistingWalletBootstrapChoicesView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
