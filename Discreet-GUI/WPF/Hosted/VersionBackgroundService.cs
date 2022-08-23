using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPF.Factories.Navigation;
using WPF.Stores;
using WPF.Views.Modals;

namespace WPF.Hosted
{
    internal class VersionBackgroundService : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly VersionUpdateStore _versionUpdateStore;

        public VersionBackgroundService(HttpClient httpClient, NavigationServiceFactory navigationServiceFactory, VersionUpdateStore versionUpdateStore)
        {
            _httpClient = httpClient;
            _navigationServiceFactory = navigationServiceFactory;
            _versionUpdateStore = versionUpdateStore;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000 * 5);

                var response = await _httpClient.GetAsync("https://releases.discreet.net/versions/wallet");

                var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                string newVersion = await response.Content.ReadAsStringAsync();
                newVersion = newVersion.Replace("\"", "");

                if (newVersion.Equals($"{currentVersion.Major}.{currentVersion.Minor}.{currentVersion.Build}")) continue;

                // New version, reset settings and display popup
                if(!string.IsNullOrWhiteSpace(_versionUpdateStore.NextVersion) && !_versionUpdateStore.NextVersion.Equals(newVersion))
                {
                    _versionUpdateStore.NextVersion = newVersion;
                    _versionUpdateStore.RemindMeLater = false;
                    _navigationServiceFactory.CreateModalNavigationService<VersionUpdateViewModel>().Navigate();
                    continue;
                }

                // Dont display untill the wallet has been restarted
                if(_versionUpdateStore.RemindMeLater == true)
                {
                    continue;
                }

                // Else set data and display popup
                _versionUpdateStore.NextVersion = newVersion;
                _navigationServiceFactory.CreateModalNavigationService<VersionUpdateViewModel>().Navigate();
            }
        }
    }
}
