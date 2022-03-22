using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WPF.Attributes;
using WPF.Factories.Navigation;
using WPF.Factories.ViewModel;
using WPF.Services.Hosted;
using WPF.Stores;
using WPF.Stores.Navigation;
using WPF.ViewModels;
using WPF.ViewModels.Account;
using WPF.ViewModels.Common;
using WPF.ViewModels.CreateWallet;
using WPF.ViewModels.Layouts;
using WPF.ViewModels.Layouts.Account;
using WPF.ViewModels.Modals;
using WPF.ViewModels.Notifications;
using WPF.ViewModels.Settings;
using WPF.ViewModels.Start;
using WPF.Views;

namespace WPF
{
    public class Startup
    {
        private static IHost _host;

        public void Run (IClassicDesktopStyleApplicationLifetime desktop)
        {
            _host = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                RegisterViewModels(services);
                RegisterFactories(services);
                RegisterStores(services);

                services.AddHostedService<DaemonActivatorService>();

                // Startup
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });
            }).Build();

            _ = _host.RunAsync();

            using IServiceScope serviceScope = _host.Services.CreateScope();

            //serviceScope.ServiceProvider.GetRequiredService<NavigationServiceFactory>().CreateModalNavigationService<TestNotificationViewModel>().Navigate();
            serviceScope.ServiceProvider.GetRequiredService<NavigationServiceFactory>().CreateModalNavigationService<PasswordViewModel>().Navigate();

            // Set the startup view
            serviceScope.ServiceProvider.GetRequiredService<NavigationServiceFactory>().Create<StartViewModel>().Navigate();
            MainWindow mainWindow = serviceScope.ServiceProvider.GetRequiredService<MainWindow>();
            desktop.MainWindow = mainWindow;
        }


        /// <summary>
        /// Assembly scan for all types with a baseType of 'ViewModelBase' and register them in the container 
        /// This will ignore any types with the class attribute 'AssemblyScanIgnore'
        /// </summary>
        /// <param name="services"></param>
        public void RegisterViewModels(IServiceCollection services)
        {
            var vms = typeof(Startup).Assembly.GetTypes().Where(t => t.BaseType == typeof(ViewModelBase) && !t.CustomAttributes.Any(t => t.AttributeType == typeof(AssemblyScanIgnoreAttribute)));
            
            foreach (Type viewModel in vms)
            {
                services.AddTransient(viewModel);
            }
        }

        public void RegisterFactories(IServiceCollection services)
        {
            services.AddScoped<NavigationServiceFactory>();
            services.AddScoped(s => new LayoutViewModelFactory(_host.Services));
        }

        public void RegisterStores(IServiceCollection services)
        {
            typeof(WindowSettingsStore).Assembly.GetTypes().Where(t => t.Name.Contains("Store") && (t.Namespace == typeof(WindowSettingsStore).Namespace || t.Namespace == typeof(MainNavigationStore).Namespace)).ToList().ForEach(t =>
            {
                services.AddSingleton(t);
            });
        }

        public static void Stop()
        {
            _ = _host.StopAsync();
            _host.Dispose();
        }
    }
}
