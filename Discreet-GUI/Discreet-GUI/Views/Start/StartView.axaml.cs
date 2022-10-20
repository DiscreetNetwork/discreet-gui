using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Discreet_GUI.Views.Start
{
    public partial class StartView : UserControl
    {
        public StartView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
