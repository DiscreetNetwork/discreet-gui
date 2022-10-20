using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Stores;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Factories.ViewModel
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
            LayoutAttribute layoutAttribute = typeof(TViewModel).GetCustomAttribute<LayoutAttribute>();

            if (layoutAttribute is null) return _serviceProvider.GetRequiredService<TViewModel>();

            layoutAttribute.Layouts = layoutAttribute.Layouts.Reverse().ToArray();

            ViewModelBase targetViewModel = _serviceProvider.GetRequiredService<TViewModel>();
            for (int i = 0; i < layoutAttribute.Layouts.Length; i++)
            {
                Type layoutType = layoutAttribute.Layouts[i];
                ConstructorInfo cinfo = layoutType.GetTypeInfo().DeclaredConstructors.FirstOrDefault();

                List<object> parameters = new List<object>();
                foreach (var pi in cinfo.GetParameters())
                {
                    if (pi.ParameterType == typeof(ViewModelBase))
                    {
                        parameters.Add(targetViewModel);
                        continue;
                    }
                    object parameter = _serviceProvider.GetRequiredService(pi.ParameterType);
                    parameters.Add(parameter);
                }

                targetViewModel = Activator.CreateInstance(layoutType, parameters.ToArray()) as ViewModelBase;
            }

            return targetViewModel;
        }

        public ViewModelBase CreateNotification<TNotificationViewModel>(string text) where TNotificationViewModel : NotificationViewModelBase 
        {
            var vm = _serviceProvider.GetRequiredService<TNotificationViewModel>();
            vm.Text = text;
            return vm;
        }
    }
}
