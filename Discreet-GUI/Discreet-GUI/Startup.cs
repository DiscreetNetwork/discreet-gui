using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Caches;
using Services.Daemon;
using Services.Testnet;
using Services.ZMQ;
using Services.ZMQ.Handlers.Common;
using Services.ZMQ.Registries;
using Services.ZMQ.Registries.Common;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Discreet_GUI.Attributes;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Factories.ViewModel;
using Discreet_GUI.Hosted;
using Discreet_GUI.Services;
using Discreet_GUI.Stores;
using Discreet_GUI.Stores.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views;
using Discreet_GUI.Views.Notifications;
using Discreet_GUI.Views.Start;
using Services.Daemon.Wallet;
using Services.Daemon.Status;
using Services.Daemon.Transaction;
using Services.Daemon.SeedRecovery;
using Services.Daemon.Read;
using Discreet_GUI.Caches;
using Splat.Microsoft.Extensions.DependencyInjection;
using Splat;
using ReactiveUI;

namespace Discreet_GUI
{
    public class Startup
    {
        private static IHost _host;
        private Subscriber _subscriber;

        public void Run (IClassicDesktopStyleApplicationLifetime desktop)
        {
            _host = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                services.UseMicrosoftDependencyResolver();
                var resolver = Locator.CurrentMutable;
                resolver.InitializeSplat();
                resolver.InitializeReactiveUI();

                DependencyBootstrapper.Register(services);

                services.AddHttpClient();

                // Startup
                services.AddSingleton<MainWindowViewModel>();
            })
            .ConfigureAppConfiguration((context, config) => BuildConfigurationFile(context, config)).Build();

            _ = _host.RunAsync();
            _host.Services.UseMicrosoftDependencyResolver();


            // ZMQ
            _subscriber = Locator.Current.GetRequiredService<Subscriber>();
            _ = Task.Factory.StartNew(_subscriber.Start);

            // Set the startup view
            Locator.Current.GetRequiredService<NavigationServiceFactory>().Create<StartViewModel>().Navigate();

            // Set daemon initializing screen, if UseActivator is enabled
            if (Locator.Current.GetRequiredService<IConfiguration>().GetValue<bool>("DaemonSettings:UseActivator"))
            {
                if(!Locator.Current.GetRequiredService<DaemonCache>().DaemonStarted)
                {
                    Locator.Current.GetRequiredService<NavigationServiceFactory>().SetDaemonStartupModal();
                }
            }

            MainWindow mainWindow = new MainWindow();
            MainWindowViewModel mainWindowViewModel = Locator.Current.GetRequiredService<MainWindowViewModel>();
            mainWindow.DataContext = mainWindowViewModel;

            desktop.MainWindow = mainWindow;
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
                catch (Exception)
                {
                }
            }

            var configData = new
            {
                DaemonSettings = new
                {
                    UseActivator = true,
                    RedirectOutput = true,
                    ExecutableName = "Discreet",
                    ExecutablePath = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) ? "/usr/lib/discreet/Discreet" : $"{Environment.ExpandEnvironmentVariables("%PROGRAMFILES%")}\\Discreet Daemon\\Discreet.exe"
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
                catch (Exception)
                {
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
                catch (Exception)
                {
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
