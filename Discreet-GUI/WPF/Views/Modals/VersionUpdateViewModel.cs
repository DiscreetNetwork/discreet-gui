using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Factories.Navigation;
using WPF.Stores;
using WPF.ViewModels.Common;

namespace WPF.Views.Modals
{
    public class VersionUpdateViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly VersionUpdateStore _versionUpdateStore;

        public string NewVersion { get => _versionUpdateStore.NextVersion; }
        public bool DisplayUpdateButton { get => Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != PlatformID.MacOSX; }
        public string UpdateMessage { get => (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) ? "Please go download the latest release build" : string.Empty; }

        public VersionUpdateViewModel(NavigationServiceFactory navigationServiceFactory, VersionUpdateStore versionUpdateStore)
        {
            _navigationServiceFactory = navigationServiceFactory;
            _versionUpdateStore = versionUpdateStore;
        }

        public void RemindMeLater()
        {
            _versionUpdateStore.RemindMeLater = true;
            _navigationServiceFactory.CreateModalNavigationService().Navigate();
        }

        public void Update()
        {
            string walletAsset = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) ? "DiscreetNetwork/discreet-gui+linux-x64.tar.gz" : "DiscreetNetwork/discreet-gui+win-x64.zip";

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = $"{Path.Combine(Directory.GetCurrentDirectory(), "utility", "Updater.exe")}",
                Arguments = $"-p \"{Path.Combine(Directory.GetCurrentDirectory(), Process.GetCurrentProcess().ProcessName)}\" -k true --grepository {walletAsset} --output \"{Directory.GetCurrentDirectory()}\"",
                UseShellExecute = true,
            };

            Process.Start(psi);
        }
    }
}
