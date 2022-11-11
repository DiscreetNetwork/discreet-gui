using System;
using System.Diagnostics;
using System.IO;
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

        public string NewVersion { get => _versionUpdateStore.NextVersion; }
        public bool DisplayUpdateButton { get => Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != PlatformID.MacOSX; }
        public string UpdateMessage { get => GetUpdaterExecutableFile() == null ? "Please go download the latest release build" : string.Empty; }
        public string ButtonText { get => GetUpdaterExecutableFile() == null ? "Continue" : "Update now"; }

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
            FileInfo updaterExecutableFileInfo = GetUpdaterExecutableFile();
            if(updaterExecutableFileInfo is null)
            {
                _versionUpdateStore.RemindMeLater = true;
                _navigationServiceFactory.CreateModalNavigationService().Navigate();
                return;
            }

            string walletAsset = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) ? "DiscreetNetwork/discreet-gui+linux-x64.tar.gz" : "DiscreetNetwork/discreet-gui+win-x64.zip";

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = updaterExecutableFileInfo.FullName,
                Arguments = $"-p \"{Path.Combine(Directory.GetCurrentDirectory(), Process.GetCurrentProcess().ProcessName)}\" -k true --grepository {walletAsset} --output \"{Directory.GetCurrentDirectory()}\"",
                UseShellExecute = true,
            };

            Process.Start(psi);
        }


        FileInfo GetUpdaterExecutableFile()
        {
            FileInfo updaterExecutableFile = new FileInfo(Path.Join(Directory.GetCurrentDirectory(), "utility", "Updater.exe"));
            if (!updaterExecutableFile.Exists)
            {
                updaterExecutableFile = new FileInfo(Path.Join(Directory.GetCurrentDirectory(), "Updater.exe"));
                if (!updaterExecutableFile.Exists)
                {
                    return null;
                }
            }

            return updaterExecutableFile;
        }
    }
}
