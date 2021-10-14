using System;
using System.Collections.Generic;
using System.Text;
using WPF.ViewModels.Common;

namespace WPF
{
    /// <summary>
    /// Attribute to use on ViewModels, to specify what layouts to combine them with.       e.g  [Layout(typeof(TitleBarLayout), typeof(FillerLayout))]
    /// First layout in the array, will be the entrypoint for the layout generation
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LayoutAttribute : Attribute
    {
        public Type[] Layouts { get; set; }

        public LayoutAttribute(params Type[] layouts)
        {
            Layouts = layouts;
        }
    }
}
