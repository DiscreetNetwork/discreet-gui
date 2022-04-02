using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.ViewModels.Common
{
    public class NotificationViewModelBase : ViewModelBase
    {
        public string NotificationId { get; set; } = Guid.NewGuid().ToString();

        public event Action<string> NotificationDismissed;

        private string _text;
        public string Text { get => _text; set { _text = value; OnPropertyChanged(nameof(Text)); } }

        public NotificationViewModelBase(string text)
        {
            Text = text;
        }

        public void DismissNotification()
        {
            NotificationDismissed?.Invoke(NotificationId);
        }
    }
}
