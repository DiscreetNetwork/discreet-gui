using Microsoft.Extensions.Hosting;
using Services.Caches;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Services;
using Discreet_GUI.Stores;
using Discreet_GUI.Views.Modals;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discreet_GUI.Hosted
{
    internal class VersionBackgroundService : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly VersionUpdateStore _versionUpdateStore;
        private readonly DaemonCache _daemonCache;
        private readonly NotificationService _notificationService;

        public VersionBackgroundService(HttpClient httpClient, NavigationServiceFactory navigationServiceFactory, VersionUpdateStore versionUpdateStore, DaemonCache daemonCache, NotificationService notificationService)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("user-agent", "Discreet GUI Wallet");
            _navigationServiceFactory = navigationServiceFactory;
            _versionUpdateStore = versionUpdateStore;
            _daemonCache = daemonCache;
            _notificationService = notificationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000 * 5);

                if (!_daemonCache.DaemonStarted) continue;

                HttpResponseMessage response = null;

                try
                {
                    response = await _httpClient.GetAsync("https://releases.discreet.net/versions/wallet");
                }
                catch (Exception)
                {
                    _notificationService.DisplayError("An error occured while trying to check for updates.");
                    continue;
                }

                if (!response.IsSuccessStatusCode) continue;

                var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                string newVersion = await response.Content.ReadAsStringAsync();
                newVersion = newVersion.Replace("\"", "");

                string[] versionNumberStrings = newVersion.Split(".");
                int major = int.Parse(versionNumberStrings[0]);
                int minor = int.Parse(versionNumberStrings[1]);
                int build = int.Parse(versionNumberStrings[2]);

                bool foundNewVersion = false;
                if (major > currentVersion.Major) foundNewVersion = true;
                else if (minor > currentVersion.Minor) foundNewVersion = true;
                if (build > currentVersion.Build) foundNewVersion = true;


                if (!foundNewVersion) continue;

                try
                {
                    var availablePackageResponse = await _httpClient.GetAsync("https://releases.discreet.net/downloads/windows/gui/latest/available");
                    if (!availablePackageResponse.IsSuccessStatusCode) continue;
                    var content = await availablePackageResponse.Content.ReadAsStringAsync();
                    var availablePackageName = JsonSerializer.Deserialize<string>(content);
                    if (!availablePackageName.Contains(newVersion)) continue;
                }
                catch (Exception)
                {
                    _notificationService.DisplayError("An error occured while trying to check for updates.");
                    continue;
                }
                
                string changelogs = await GetChangelogs($"v{newVersion}");
                _versionUpdateStore.Changelogs = changelogs;

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

        async Task<string> GetChangelogs(string tagToMatch)
        {
            var response = await _httpClient.GetAsync($"https://api.github.com/repos/discreetnetwork/discreet-gui/releases/latest");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var release = JsonSerializer.Deserialize<GithubRelease>(content);

            if (tagToMatch.ToLower() != release.Tag.ToLower()) return null;

            return release.Changelogs == "CHANGELOG" ? null : release.Changelogs;
        }


        private class GithubRelease
        {
            [JsonPropertyName("tag_name")]
            public string Tag { get; set; }

            [JsonPropertyName("body")]
            public string Changelogs { get; set; }
        }
    }
}
