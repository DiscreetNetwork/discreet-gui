﻿using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using WPF.Factories.Navigation;
using WPF.Factories.ViewModel;
using WPF.Stores.Navigation;
using WPF.ViewModels;
using WPF.ViewModels.Start;
using WPF.Views;

namespace WPF
{
    public class Startup
    {
        private IHost _host;

        public void Run (IClassicDesktopStyleApplicationLifetime desktop)
        {
            _host = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                RegisterViewModels(services);
                RegisterFactories(services);
                RegisterStores(services);

                // Startup
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });
            }).Build();

            using IServiceScope serviceScope = _host.Services.CreateScope();

            serviceScope.ServiceProvider.GetRequiredService<NavigationServiceFactory>().Create<StartViewModel>().Navigate();
            MainWindow mainWindow = serviceScope.ServiceProvider.GetRequiredService<MainWindow>();
            desktop.MainWindow = mainWindow;
        }

        public void RegisterViewModels(IServiceCollection services)
        {
            // Start ViewModels
            services.AddTransient<StartViewModel>();
        }

        public void RegisterFactories(IServiceCollection services)
        {
            services.AddScoped<NavigationServiceFactory>();
            services.AddScoped(s => new LayoutViewModelFactory(_host.Services));
        }

        public void RegisterStores(IServiceCollection services)
        {
            services.AddSingleton<MainNavigationStore>();
        }
    }
}
