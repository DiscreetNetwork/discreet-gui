using System;
using System.Collections.Generic;
using System.Text;
using WPF.Attributes;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Layouts
{
    [AssemblyScanIgnore("This is a LayoutViewModel and is used by other ViewModels, but never alone and thus it shouldnt be registered")]
    public class StartLayoutViewModel : ViewModelBase
    {
        public ViewModelBase ContentViewModel { get; set; }


        public StartLayoutViewModel(ViewModelBase contentViewModel)
        {
            ContentViewModel = contentViewModel;
        }
    }
}
