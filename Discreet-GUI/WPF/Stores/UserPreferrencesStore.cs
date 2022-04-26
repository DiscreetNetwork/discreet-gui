using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Stores
{
    /// <summary>
    /// Contains values for user preferrences
    /// ViewModels can inject this Store and bind to the properties, to be notified of changes
    /// </summary>
    public class UserPreferrencesStore
    {
        public event Action HideBalanceChanged;
        private bool _hideBalance = false;
        public bool HideBalance { get => _hideBalance; set { _hideBalance = value; HideBalanceChanged?.Invoke(); } }
    }
}
