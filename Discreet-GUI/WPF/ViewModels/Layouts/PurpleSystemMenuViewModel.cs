using System;
using System.Collections.Generic;
using System.Text;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Layouts
{
    public class PurpleSystemMenuViewModel : ViewModelBase
    {
        public ViewModelBase ContentViewModel { get; }

        public PurpleSystemMenuViewModel(ViewModelBase contentViewModel)
        {
            ContentViewModel = contentViewModel;
        }
    }
}
