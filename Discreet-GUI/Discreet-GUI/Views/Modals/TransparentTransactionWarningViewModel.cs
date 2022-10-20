using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Views.Modals
{
    public class TransparentTransactionWarningViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;

        public TransparentTransactionWarningViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            _navigationServiceFactory = navigationServiceFactory;
        }

        public void Cancel()
        {
            _navigationServiceFactory.CreateModalNavigationService().Navigate();
        }

        public void Continue()
        {
            _navigationServiceFactory.CreateModalNavigationService<ConfirmViewModel>().Navigate();
        }
    }
}
