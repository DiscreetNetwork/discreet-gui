using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace WPF.Views.Layouts
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

        void ClickHandler(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
