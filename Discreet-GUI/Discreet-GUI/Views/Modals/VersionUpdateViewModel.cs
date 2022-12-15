using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Stores;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Layouts;

namespace Discreet_GUI.Views.Modals
{
    [Layout(typeof(PurpleTitleBarLayoutViewModel))]
    public class VersionUpdateViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly VersionUpdateStore _versionUpdateStore;
        private readonly HttpClient _httpClient;

        //public bool DisplayUpdateButton { get => Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != PlatformID.MacOSX; }

        private bool _downloading = false;
        public bool Downloading { get => _downloading; set { _downloading = value; OnPropertyChanged(nameof(Downloading)); } }

        private double _downloadPercent = 0;
        public double DownloadPercent { get => _downloadPercent; set { _downloadPercent = value; OnPropertyChanged(nameof(DownloadPercent)); } }

        private bool _downloadCompleted = false;
        public bool DownloadCompleted { get => _downloadCompleted; set { _downloadCompleted = value; OnPropertyChanged(nameof(DownloadCompleted)); } }

        private bool _fileSaved = false;
        public bool FileSaved { get => _fileSaved; set { _fileSaved = value; OnPropertyChanged(nameof(FileSaved)); } }

        private string _statusText = string.Empty;
        public string StatusText { get => _statusText; set { _statusText = value; OnPropertyChanged(nameof(StatusText)); } }

        public bool ChangelogsAvailable { get; set; }
        public string Changelogs => _versionUpdateStore.Changelogs;

        private string _fileName = string.Empty;
        private byte[] _fileBytes = null;

        public VersionUpdateViewModel(NavigationServiceFactory navigationServiceFactory, VersionUpdateStore versionUpdateStore, HttpClient httpClient)
        {
            _navigationServiceFactory = navigationServiceFactory;
            _versionUpdateStore = versionUpdateStore;
            _httpClient = httpClient;
            StatusText = _versionUpdateStore.NextVersion;
            ChangelogsAvailable = !string.IsNullOrWhiteSpace(Changelogs);
        }

        public void RemindMeLater()
        {
            _versionUpdateStore.RemindMeLater = true;
            _navigationServiceFactory.CreateModalNavigationService().Navigate();
        }

        
        async Task Download()
        {
            Downloading = true;
            StatusText = "Downloading the new update..";


            var downloadEndpoint = Environment.OSVersion.Platform == PlatformID.Unix ?
                "https://releases.discreet.net/downloads/debian/gui/latest/" :
                "https://releases.discreet.net/downloads/windows/gui/latest/";

            var response = await _httpClient.GetAsync(downloadEndpoint, HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
            {
                StatusText = "Something went wrong, restart the wallet and try again.";
                return;
            }

            _fileName = response.Content.Headers.ContentDisposition.FileName;
            var fileLength = response.Content.Headers.ContentLength;
            using var stream = await response.Content.ReadAsStreamAsync();


            var totalBytesRead = 0;
            var buffer = new byte[4096];
            _fileBytes = new byte[fileLength.Value];
            while (totalBytesRead < fileLength)
            {
                var read = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (read == 0)
                {
                    StatusText = "Something went wrong, restart the wallet and try again.";
                    return;
                }

                Buffer.BlockCopy(buffer, 0, _fileBytes, totalBytesRead, read);
                totalBytesRead += read;

                DownloadPercent = Math.Round((double)totalBytesRead / fileLength.Value * 100, 2);
            }

            if (totalBytesRead != fileLength.Value)
            {
                StatusText = "Something went wrong, restart the wallet and try again.";
                return;
            }


            DownloadCompleted = true;
            StatusText = "Download completed. Save the new update.";
        }

        async Task Save()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialFileName = _fileName;
            saveFileDialog.Title = "Save the new build";

            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                try
                {
                    var destionationPath = await saveFileDialog.ShowAsync(desktop.MainWindow);
                    if (string.IsNullOrWhiteSpace(destionationPath)) return;

                    using var filestream = new FileStream(destionationPath, FileMode.Create, FileAccess.Write, FileShare.None);
                    await filestream.WriteAsync(_fileBytes);
                }
                catch (Exception)
                {
                    StatusText = "Something went wrong, restart the wallet and try again.";
                    return;
                }
            }

            DownloadCompleted = false;
            FileSaved = true;
            StatusText = "Close the wallet and apply the new update!";
        }

        void Continue()
        {
            _navigationServiceFactory.CreateModalNavigationService().Navigate();
        }




        
    }
}
