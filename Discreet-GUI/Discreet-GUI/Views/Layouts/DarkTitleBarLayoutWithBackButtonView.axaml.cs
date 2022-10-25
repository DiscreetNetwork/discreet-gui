using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Discreet_GUI.Views.Layouts
{
    public partial class DarkTitleBarLayoutWithBackButtonView : UserControl
    {
        public DarkTitleBarLayoutWithBackButtonView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
