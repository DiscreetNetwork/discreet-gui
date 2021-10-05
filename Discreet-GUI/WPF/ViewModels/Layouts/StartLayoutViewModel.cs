using System;
using System.Collections.Generic;
using System.Text;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Layouts
{
    public class StartLayoutViewModel : ViewModelBase
    {
        public ViewModelBase ContentViewModel { get; set; }

        public StartLayoutViewModel(ViewModelBase contentViewModel)
        {
            ContentViewModel = contentViewModel;
        }
    }
}
