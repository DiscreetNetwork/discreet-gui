using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using WPF.ViewModels;
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

            }).Build();

            using IServiceScope serviceScope = _host.Services.CreateScope();

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }
    }
}
