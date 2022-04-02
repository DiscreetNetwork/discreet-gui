using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WPF.Views.Notifications
{
    public partial class NotificationContainerView : UserControl
    {
        public NotificationContainerView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
