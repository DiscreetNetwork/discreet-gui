using Avalonia;
using Avalonia.Controls;
using System;

namespace Discreet_GUI.Utility
{
    public class ProgressBarBehaviour
    {
        public static AvaloniaProperty<double> ValueProperty = AvaloniaProperty.RegisterAttached<ProgressBarBehaviour, ProgressBar, double>("Value");

        public static void SetValue(ProgressBar pb, double value) => pb.SetValue(ValueProperty, value);

        static ProgressBarBehaviour()
        {
            ValueProperty.Changed.Subscribe(ev => { ((ProgressBar)ev.Sender).Value = ev.NewValue.Value; });
        }
    }
}
