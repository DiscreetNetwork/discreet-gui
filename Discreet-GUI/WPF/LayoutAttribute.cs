using System;
using System.Collections.Generic;
using System.Text;
using WPF.ViewModels.Common;

namespace WPF
{
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
