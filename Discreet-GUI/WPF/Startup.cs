using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Daemon;
using Services.Daemon.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using WPF.Attributes;
using WPF.Caches;
using WPF.Factories.Navigation;
using WPF.Factories.ViewModel;
using WPF.Hosted;
using WPF.Services;
using WPF.Stores;
using WPF.Stores.Navigation;
using WPF.ViewModels;
using WPF.ViewModels.Common;
using WPF.Views;
using WPF.Views.Account.Modals;
using WPF.Views.DebugUtility;
using WPF.Views.Notifications;
using WPF.Views.Start;

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
                RegisterNotificationViewModels(services);
                RegisterFactories(services);
                RegisterStores(services);
                RegisterCaches(services);

                services.AddSingleton<NotificationContainerViewModel>();
                services.AddSingleton<RPCServer>();
                services.AddSingleton<NotificationService>();
                services.AddHostedService<DaemonActivatorService>();
                services.AddHostedService<WalletPollerBackgroundService>();
                services.AddSingleton<WalletService>();
                services.AddSingleton<StatusService>();


                // Startup
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                var walletConfigPath = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) ? Environment.GetEnvironmentVariable("HOME") : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
                walletConfigPath = Path.Combine($"{walletConfigPath}", "discreet/wallet-config");
                if (!Directory.Exists(walletConfigPath))
                {
                    try
                    {
                        Directory.CreateDirectory(walletConfigPath);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"WPF.Startup.ConfigureAppConfiguration - Failed to create default wallet configuration folder: \n{e.Message}");
                    }
                }
                if (!File.Exists(Path.Combine(walletConfigPath, "appsettings.json")))
                {
                    try
                    {
                        using var fs = File.Create(Path.Combine(walletConfigPath, "appsettings.json"));
                        using var sw = new StreamWriter(fs);
                        sw.Write($"{{\n  \"DaemonSettings\": {{\n    \"UseActivator\": true,\n    \"RedirectOutput\": true,\n    \"ExecutableName\": \"Discreet\",\n    \"ExecutablePath\": \"path to Discreet.exe\"\n  }}\n}}");
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"WPF.Startup.ConfigureAppConfiguration - Failed to create default wallet appsettings.json file: \n{e.Message}");
                    }
                }

                config.AddJsonFile(Path.Combine(walletConfigPath, "appsettings.json"));
#if DEBUG
                if (!Directory.Exists(walletConfigPath))
                {
                    try
                    {
                        Directory.CreateDirectory(walletConfigPath);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"WPF.Startup.ConfigureAppConfiguration - Failed to create default wallet configuration folder: \n{e.Message}");
                    }
                }
                if (!File.Exists(Path.Combine(walletConfigPath, "debugsettings.json")))
                {
                    try
                    {
                        using var fs = File.Create(Path.Combine(walletConfigPath, "debugsettings.json"));
                        using var sw = new StreamWriter(fs);
                        sw.Write($"{{\n  \"FaucetRemoteNode\": \"http://ip:port/\"\n}}");
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"WPF.Startup.ConfigureAppConfiguration - Failed to create default debugsettings.json file: \n{e.Message}");
                    }
                }
                config.AddJsonFile(Path.Combine(walletConfigPath, "debugsettings.json"));
#endif
            }).Build();

            _ = _host.RunAsync();

            using IServiceScope serviceScope = _host.Services.CreateScope();

            // Set the startup view
            serviceScope.ServiceProvider.GetRequiredService<NavigationServiceFactory>().Create<StartViewModel>().Navigate();

            //if(serviceScope.ServiceProvider.GetRequiredService<IConfiguration>().GetValue<bool>("DaemonSettings:UseActivator"))
            //{
            //    serviceScope.ServiceProvider.GetRequiredService<NavigationServiceFactory>().CreateModalNavigationService<LoadingSpinnerViewModel>().Navigate();
            //}

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

        public void RegisterNotificationViewModels(IServiceCollection services)
        {
            var vms = typeof(Startup).Assembly.GetTypes().Where(t => t.BaseType == typeof(NotificationViewModelBase) && !t.CustomAttributes.Any(t => t.AttributeType == typeof(AssemblyScanIgnoreAttribute)));

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

        public void RegisterCaches(IServiceCollection services)
        {
            typeof(NewWalletCache).Assembly.GetTypes().Where(t => t.Name.Contains("Cache") && (t.Namespace == typeof(NewWalletCache).Namespace)).ToList().ForEach(t =>
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
