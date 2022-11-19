using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Discreet_GUI.Views.CreateWallet
{
    public partial class YourRecoveryPhraseView : ReactiveUserControl<YourRecoveryPhraseViewModel>
    {
        public YourRecoveryPhraseView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
