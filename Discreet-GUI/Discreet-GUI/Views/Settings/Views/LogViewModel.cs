using Services.Caches;
using System.Collections.ObjectModel;
using System.Linq;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Views.Settings.Views
{
    public class LogViewModel : ViewModelBase
    {
        private readonly DaemonLogCache _daemonLogCache;

        public ObservableCollection<string> Logs { get; set; }

        public ObservableCollection<string> LogTypes { get; set; } = new ObservableCollection<string> { "All", "Info", "Debug" };

        private int _selectedLogTypeIndex = 0;
        public int SelectedLogTypeIndex { get => _selectedLogTypeIndex; set { _selectedLogTypeIndex = value; LogTypeChangedHandler(); } }

        public LogViewModel(DaemonLogCache daemonLogCache)
        {
            _daemonLogCache = daemonLogCache;
            Logs = new ObservableCollection<string>(_daemonLogCache.Logs);

            _daemonLogCache.LogsModified += () => LogTypeChangedHandler();
        }

        void LogTypeChangedHandler()
        {
            string type = LogTypes[SelectedLogTypeIndex];

            switch (type)
            {
                case "All":
                    Logs = new ObservableCollection<string>(_daemonLogCache.Logs);
                    break;

                case "Info":
                    Logs = new ObservableCollection<string>(_daemonLogCache.Logs.Where(s => s.Contains("[INFO]")));
                    break;

                case "Debug":
                    Logs = new ObservableCollection<string>(_daemonLogCache.Logs.Where(s => s.Contains("[DEBUG]")));
                    break;

                default:
                    break;
            }

            OnPropertyChanged(nameof(Logs));
        }
    }
}
