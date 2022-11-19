using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Discreet_GUI.Views.Modals
{
    public partial class LoadingSpinnerView : ReactiveUserControl<LoadingSpinnerViewModel>
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
