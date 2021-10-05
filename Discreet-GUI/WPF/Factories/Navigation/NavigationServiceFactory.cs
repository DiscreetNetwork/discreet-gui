﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WPF.Services.Navigation.Common;
using WPF.Stores.Navigation;
using WPF.ViewModels;

namespace WPF.Factories.Navigation
{
    /// <summary>
    /// This factory is responsible for constructing NavigationServices based on the specified Type of ViewModelBase
    /// </summary>
    public class NavigationServiceFactory
    {
        private readonly MainNavigationStore _mainNavigationStore;

        public NavigationServiceFactory(IServiceProvider serviceProvider)
        {
            _mainNavigationStore = serviceProvider.GetRequiredService<MainNavigationStore>();
        }

        public INavigationService Create<TViewModel>() where TViewModel : ViewModelBase
        {
            throw new InvalidOperationException();
        }
    }
}