using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Discreet_GUI.Views.Account.Modals
{
    public partial class TransactionDetailsView : UserControl
    {
        public TransactionDetailsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
