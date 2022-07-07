using Avalonia;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
//using DesktopNotifications.Avalonia;

namespace WPF
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += GlobalExceptionHandler.UnhandledExceptionHandler;
            TaskScheduler.UnobservedTaskException += GlobalExceptionHandler.UnobservedTaskExceptionHandler;
            RxApp.DefaultExceptionHandler = GlobalExceptionHandler.GetReactiveObserverExceptionHandler();

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                //.SetupDesktopNotifications()
                .LogToTrace()
                .UseReactiveUI();
    }
}
