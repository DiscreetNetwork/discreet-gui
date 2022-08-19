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
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = $"{Path.Combine(Directory.GetCurrentDirectory(), "Updater.exe")}",
                Arguments = $"-p {Process.GetCurrentProcess().ProcessName} -k true --grepository DiscreetNetwork/discreet-gui+win-x64.zip",
                UseShellExecute = true
            };

            Process.Start(psi);
        }
    }
}
