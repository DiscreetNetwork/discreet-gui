using System;

namespace Discreet_GUI.Stores
{
    public class VersionUpdateStore
    {
        public event Action NextVersionChanged;
        private string _nextVersion = string.Empty;
        public string NextVersion { get => _nextVersion; set { _nextVersion = value; OnNextVersionChanged(); } }
        public void OnNextVersionChanged() => NextVersionChanged?.Invoke();



        public event Action RemindMeLaterChanged;
        private bool _remindMeLater = false;
        public bool RemindMeLater { get => _remindMeLater; set { _remindMeLater = value; OnRemindMeLaterChanged(); } }
        public void OnRemindMeLaterChanged() => RemindMeLaterChanged?.Invoke();


        public string Changelogs { get; set; }
    }
}
