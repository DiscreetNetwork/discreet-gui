using Avalonia;
using Avalonia.Controls;
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
            // Handled returns True for controls with a MouseDown Event, in which case we don't want to drag the window
            if (e.Handled) return; 

            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                this.BeginMoveDrag(e);
        }
    }
}
