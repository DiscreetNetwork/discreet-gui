using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WPF.Views.Modals
{
    public partial class TransparentTransactionWarningView : UserControl
    {
        public TransparentTransactionWarningView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
