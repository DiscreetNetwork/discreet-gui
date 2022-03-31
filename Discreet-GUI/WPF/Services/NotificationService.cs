using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;

namespace WPF.Services
{
    public class NotificationService
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;

        public NotificationService(NavigationServiceFactory navigationServiceFactory)
        {
            _navigationServiceFactory = navigationServiceFactory;
        }

        public void Display(string content, NotificationSeverity severity)
        {

        }

        public void Display<TViewModel>() where TViewModel : ViewModelBase => _navigationServiceFactory.DisplayNotification<TViewModel>().Navigate();
        public void Dismiss() => _navigationServiceFactory.DismissNotification().Navigate();
    }

    public enum NotificationSeverity
    {
        Success
    }
}
