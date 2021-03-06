using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Services.Caches;
using Services.Daemon;
using Services.Daemon.Services;
using Services.Testnet;
using Services.ZMQ;
using Services.ZMQ.Handlers.Common;
using Services.ZMQ.Registries;
using Services.ZMQ.Registries.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WPF.Attributes;
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
using WPF.Views.Modals;
using WPF.Views.Notifications;
using WPF.Views.Start;

namespace WPF
{
    public class Startup
    {
        private static IHost _host;
        private Subscriber _subscriber;

        public void Run (IClassicDesktopStyleApplicationLifetime desktop)
        {
            _host = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                RegisterViewModels(services);
                RegisterNotificationViewModels(services);
                RegisterFactories(services);
                RegisterStores(services);
                RegisterCaches(services);

                // ZMQ Dependencies
                var handlers = typeof(ServiceProviderMessageHandlerRegistry).Assembly.GetTypes().Where(t => t.BaseType == typeof(MessageHandler));
                foreach (var handler in handlers)
                {
                    services.AddScoped(handler);
                }
                services.AddSingleton<IMessageHandlerRegistry, ServiceProviderMessageHandlerRegistry>();
                services.AddScoped<Subscriber>();

                services.AddHttpClient();

                services.AddSingleton<NotificationContainerViewModel>();
                services.AddScoped<RPCServer>();
                services.AddSingleton<NotificationService>();
                services.AddHostedService<DaemonActivatorService>();
                services.AddHostedService<WalletPollerBackgroundService>();
                services.AddScoped<WalletService>();
                services.AddScoped<StatusService>();
                services.AddScoped<AccountService>();
                services.AddScoped<IssueService>();

                // Startup
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });
            })
            .ConfigureAppConfiguration((context, config) => BuildConfigurationFile(context, config)).Build();

            _ = _host.RunAsync();

            using IServiceScope serviceScope = _host.Services.CreateScope();

            // ZMQ
            _subscriber = serviceScope.ServiceProvider.GetRequiredService<Subscriber>();
            _ = Task.Factory.StartNew(_subscriber.Start);

            // Set the startup view
            serviceScope.ServiceProvider.GetRequiredService<NavigationServiceFactory>().Create<StartViewModel>().Navigate();

            if (serviceScope.ServiceProvider.GetRequiredService<IConfiguration>().GetValue<bool>("DaemonSettings:UseActivator"))
            {
                serviceScope.ServiceProvider.GetRequiredService<NavigationServiceFactory>().CreateModalNavigationService<LoadingSpinnerViewModel>().Navigate();
            }

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


        /// <summary>
        /// Builds the configuration file that is to be loaded in on startup
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
        public void BuildConfigurationFile(HostBuilderContext context, IConfigurationBuilder config)
        {
            var walletConfigPath = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) ? Environment.GetEnvironmentVariable("HOME") : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            walletConfigPath = Path.Combine($"{walletConfigPath}", "discreet/wallet-config");

            FileInfo configFileInfo = new FileInfo(Path.Combine(walletConfigPath, "appsettings.json"));

            // Ensure the directory exists
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

            var configData = new
            {
                DaemonSettings = new
                {
                    UseActivator = true,
                    RedirectOutput = true,
                    ExecutableName = "Discreet",
                    ExecutablePath = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) ? "Discreet.exe" : $"{Environment.ExpandEnvironmentVariables("%PROGRAMFILES(X86)%")}\\Discreet Daemon\\Discreet.exe"
                },

                ZMQSettings = new
                {
                    SubscriberPort = 26833
                }
            };


            if (!configFileInfo.Exists)
            {
                try
                {
                    using var fs = File.Create(configFileInfo.FullName);
                    using var sw = new StreamWriter(fs);
                    sw.Write(JsonSerializer.Serialize(configData, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                    }));
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"WPF.Startup.ConfigureAppConfiguration - Failed to create default wallet appsettings.json file: \n{e.Message}");
                }
            }


            config.AddJsonFile(configFileInfo.FullName);
#if DEBUG
            FileInfo debugConfigFileInfo = new FileInfo(Path.Combine(walletConfigPath, "debugsettings.json"));

            var debugConfigData = new
            {
                FaucetRemoteNode = "http://ip:port"
            };

            if (!debugConfigFileInfo.Exists)
            {
                try
                {
                    using var fs = File.Create(Path.Combine(walletConfigPath, "debugsettings.json"));
                    using var sw = new StreamWriter(fs);
                    sw.Write(JsonSerializer.Serialize(debugConfigData, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                    }));
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"WPF.Startup.ConfigureAppConfiguration - Failed to create default debugsettings.json file: \n{e.Message}");
                }
            }

            config.AddJsonFile(debugConfigFileInfo.FullName);
#endif
        }

        public static void Stop()
        {
            _ = _host.StopAsync();
            _host.Dispose();
        }
    }
}
