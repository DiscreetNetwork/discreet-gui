using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System.Windows.Input;

namespace WPF.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.AddHandler(PointerPressedEvent, MouseDownHandler, handledEventsToo: true);

#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void MouseDownHandler(object sender, PointerPressedEventArgs e)
        {
            var pos = e.GetCurrentPoint(this);
            if (e.Handled || pos.Position.Y > 100) return;

            // Dont allow drag, if the window is maximized
            if (WindowState == WindowState.Maximized) return;

            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                this.BeginMoveDrag(e);
        }
    }
}
