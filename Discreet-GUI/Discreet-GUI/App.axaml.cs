using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Discreet_GUI.ViewModels;
using Discreet_GUI.Views;

namespace Discreet_GUI
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                new Startup().Run(desktop);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
