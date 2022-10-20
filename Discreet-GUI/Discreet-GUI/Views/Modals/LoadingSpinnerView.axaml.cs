using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Discreet_GUI.Views.Modals
{
    public partial class LoadingSpinnerView : UserControl
    {
        public LoadingSpinnerView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
