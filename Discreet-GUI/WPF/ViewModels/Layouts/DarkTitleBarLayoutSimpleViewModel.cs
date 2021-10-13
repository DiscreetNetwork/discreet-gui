using System;
using System.Collections.Generic;
using System.Text;
using WPF.Factories.Navigation;
using WPF.Stores;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Layouts
{
    class DarkTitleBarLayoutSimpleViewModel : TitleBarViewModelBase
    {
        public DarkTitleBarLayoutSimpleViewModel(ViewModelBase contentViewModel, WindowSettingsStore windowSettingsStore) : base(contentViewModel, windowSettingsStore)
        {

        }
    }
}
