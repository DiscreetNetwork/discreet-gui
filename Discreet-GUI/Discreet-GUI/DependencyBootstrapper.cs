using Discreet_GUI.Attributes;
using Discreet_GUI.Caches;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Factories.ViewModel;
using Discreet_GUI.Hosted;
using Discreet_GUI.Services;
using Discreet_GUI.Stores;
using Discreet_GUI.Stores.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Services.Caches;
using Services.Daemon;
using Services.Daemon.Read;
using Services.Daemon.SeedRecovery;
using Services.Daemon.Status;
using Services.Daemon.Transaction;
using Services.Daemon.Wallet;
using Services.Testnet;
using Services.ZMQ;
using Services.ZMQ.Handlers.Common;
using Services.ZMQ.Registries;
using Services.ZMQ.Registries.Common;
using System;
using System.Linq;

namespace Discreet_GUI
{
    internal class DependencyBootstrapper
    {
        internal static void Register(IServiceCollection services)
        {
            RegisterViewModels(services);
            RegisterNotificationViewModels(services);
            RegisterFactories(services);
            RegisterStores(services);
            RegisterCaches(services);
            RegisterZMQ(services);
            RegisterServices(services);
        }




        /// <summary>
        /// Assembly scan for all types with a baseType of 'ViewModelBase' and register them in the container 
        /// This will ignore any types with the class attribute 'AssemblyScanIgnore'
        /// </summary>
        /// <param name="services"></param>
        static void RegisterViewModels(IServiceCollection services)
        {
            var vms = typeof(Startup).Assembly.GetTypes().Where(t => t.BaseType == typeof(ViewModelBase) && !t.CustomAttributes.Any(t => t.AttributeType == typeof(AssemblyScanIgnoreAttribute)));

            foreach (Type viewModel in vms)
            {
                services.AddTransient(viewModel);
            }

            services.AddSingleton<NotificationContainerViewModel>();
        }

        static void RegisterNotificationViewModels(IServiceCollection services)
        {
            var vms = typeof(Startup).Assembly.GetTypes().Where(t => t.BaseType == typeof(NotificationViewModelBase) && !t.CustomAttributes.Any(t => t.AttributeType == typeof(AssemblyScanIgnoreAttribute)));

            foreach (Type viewModel in vms)
            {
                services.AddTransient(viewModel);
            }
        }

        static void RegisterFactories(IServiceCollection services)
        {
            services.AddScoped<NavigationServiceFactory>();
            services.AddScoped<LayoutViewModelFactory>();
        }

        static void RegisterStores(IServiceCollection services)
        {
            typeof(WindowSettingsStore).Assembly.GetTypes().Where(t => t.Name.Contains("Store") && (t.Namespace == typeof(WindowSettingsStore).Namespace || t.Namespace == typeof(MainNavigationStore).Namespace)).ToList().ForEach(t =>
            {
                services.AddSingleton(t);
            });
        }

        static void RegisterCaches(IServiceCollection services)
        {
            typeof(SubmitIssueCache).Assembly.GetTypes().Where(t => t.Name.Contains("Cache") && (t.Namespace == typeof(SubmitIssueCache).Namespace)).ToList().ForEach(t =>
            {
                services.AddSingleton(t);
            });

            typeof(NewWalletCache).Assembly.GetTypes().Where(t => t.Name.Contains("Cache") && (t.Namespace == typeof(NewWalletCache).Namespace)).ToList().ForEach(t =>
            {
                services.AddSingleton(t);
            });
        }

        static void RegisterZMQ(IServiceCollection services)
        {
            var handlers = typeof(ServiceProviderMessageHandlerRegistry).Assembly.GetTypes().Where(t => t.BaseType == typeof(MessageHandler));
            foreach (var handler in handlers)
            {
                services.AddScoped(handler);
            }

            services.AddSingleton<IMessageHandlerRegistry, ServiceProviderMessageHandlerRegistry>();
            services.AddScoped<Subscriber>();
        }

        static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<RPCServer>();
            services.AddSingleton<NotificationService>();
            services.AddHostedService<DaemonActivatorService>();
            services.AddHostedService<WalletPollerBackgroundService>();
            services.AddHostedService<VersionBackgroundService>();
            services.AddScoped<DaemonWalletService>();
            services.AddScoped<DaemonTransactionService>();
            services.AddScoped<DaemonStatusService>();
            services.AddScoped<DaemonSeedRecoveryService>();
            services.AddScoped<DaemonReadService>();
            services.AddScoped<IssueService>();
        }
    }
}
