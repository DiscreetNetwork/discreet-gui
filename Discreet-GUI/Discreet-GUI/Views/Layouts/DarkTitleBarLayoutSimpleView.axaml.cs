using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Discreet_GUI.Views.Layouts
{
    public partial class DarkTitleBarLayoutSimpleView : UserControl
    {
        public DarkTitleBarLayoutSimpleView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
