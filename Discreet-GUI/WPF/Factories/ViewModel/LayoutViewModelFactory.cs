using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WPF.Stores;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;
using WPF.ViewModels.Start;

namespace WPF.Factories.ViewModel
{
    /// <summary>
    /// A class that is responsible for creating and combining ViewModels based on a Type and a LayoutViewModel
    /// </summary>
    public class LayoutViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WindowSettingsStore _windowSettingsStore;

        public LayoutViewModelFactory(IServiceProvider serviceProvider, WindowSettingsStore windowSettingsStore)
        {
            _serviceProvider = serviceProvider;
            _windowSettingsStore = windowSettingsStore;
        }

        public ViewModelBase Create<TViewModel>() where TViewModel : ViewModelBase
        {
            if (typeof(TViewModel) == typeof(StartViewModel)) 
                return new PurpleTitleBarLayoutViewModel(
                    new StartLayoutViewModel(
                    _serviceProvider.GetRequiredService<TViewModel>()), _windowSettingsStore);

            if (typeof(TViewModel) == typeof(CreateWalletChoicesViewModel))
                return new PurpleTitleBarLayoutViewModel(
                    new StartLayoutViewModel(
                        _serviceProvider.GetRequiredService<TViewModel>()), _windowSettingsStore);

            if (typeof(TViewModel) == typeof(YourRecoveryPhraseViewModel))
                return new DarkTitleBarLayoutViewModel(_serviceProvider.GetRequiredService<TViewModel>(), _windowSettingsStore);

            throw new InvalidOperationException();
        }
    }
}
