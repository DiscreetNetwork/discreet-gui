using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WPF.Views.Modals
{
    public partial class VersionUpdateView : UserControl
    {
        public VersionUpdateView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
