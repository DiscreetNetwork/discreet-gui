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

            this.AddHandler(PointerPressedEvent, MouseDownHandler, handledEventsToo: false);

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

            // A fix to ensure the window wont begin drag, if you click on a ComboBoxItem
            if (pos.Pointer.Captured.InteractiveParent is Avalonia.Controls.ComboBoxItem) return;

            if (pos.Position.Y > 100 || e.Handled) return;

            // Dont allow drag, if the window is maximized
            if (WindowState == WindowState.Maximized) return;

            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                this.BeginMoveDrag(e);
        }
    }
}
