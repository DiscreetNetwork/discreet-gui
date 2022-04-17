using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WPF.Views.CreateWallet
{
    public partial class VerifyRecoveryPhraseView : UserControl
    {
        public VerifyRecoveryPhraseView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
