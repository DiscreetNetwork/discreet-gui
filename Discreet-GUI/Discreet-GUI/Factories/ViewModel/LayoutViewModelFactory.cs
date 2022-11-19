using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Discreet_GUI.ViewModels.Common;
using Splat;

namespace Discreet_GUI.Factories.ViewModel
{
    /// <summary>
    /// A class that is responsible for creating and combining ViewModels based on a Type and a LayoutViewModel
    /// </summary>
    public class LayoutViewModelFactory
    {
        public ViewModelBase Create<TViewModel>() where TViewModel : ViewModelBase
        {
            LayoutAttribute layoutAttribute = typeof(TViewModel).GetCustomAttribute<LayoutAttribute>();

            if (layoutAttribute is null) return Locator.Current.GetRequiredService<TViewModel>();

            layoutAttribute.Layouts = layoutAttribute.Layouts.Reverse().ToArray();

            ViewModelBase targetViewModel = Locator.Current.GetRequiredService<TViewModel>();
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
                    object parameter = Locator.Current.GetRequiredService(pi.ParameterType);
                    parameters.Add(parameter);
                }

                targetViewModel = Activator.CreateInstance(layoutType, parameters.ToArray()) as ViewModelBase;
            }

            return targetViewModel;
        }

        public ViewModelBase CreateNotification<TNotificationViewModel>(string text) where TNotificationViewModel : NotificationViewModelBase 
        {
            var vm = Locator.Current.GetRequiredService<TNotificationViewModel>();
            vm.Text = text;
            return vm;
        }
    }
}
