using System;
using System.Collections.Generic;
using System.Text;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Layouts
{
    public class PurpleSystemMenuLayoutViewModel : ViewModelBase
    {
        public ViewModelBase ContentViewModel { get; }

        public PurpleSystemMenuLayoutViewModel(ViewModelBase contentViewModel)
        {
            ContentViewModel = contentViewModel;
        }
    }
}
