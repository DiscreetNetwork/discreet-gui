using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WPF.ViewModels.Common;
using WPF.ViewModels.Start;

namespace WPF.Factories.ViewModel
{
    /// <summary>
    /// A class that is responsible for creating and combining ViewModels based on a Type and a LayoutViewModel
    /// </summary>
    public class LayoutViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public LayoutViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ViewModelBase Create<TViewModel>() where TViewModel : ViewModelBase
        {
            if (typeof(TViewModel) == typeof(StartViewModel)) return _serviceProvider.GetRequiredService<TViewModel>();

            throw new InvalidOperationException();
        }
    }
}
