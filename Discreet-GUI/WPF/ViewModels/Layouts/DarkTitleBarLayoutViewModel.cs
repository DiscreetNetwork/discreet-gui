using System;
using System.Collections.Generic;
using System.Text;
using WPF.Stores;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Layouts
{
    public class DarkTitleBarLayoutViewModel : TitleBarViewModelBase
    {
        public DarkTitleBarLayoutViewModel(ViewModelBase contentViewModel, WindowSettingsStore windowSettingsStore) : base(contentViewModel, windowSettingsStore)
        {

        }
    }
}
