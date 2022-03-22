using System;
using System.Collections.Generic;
using System.Text;

namespace WPF.Attributes
{
    /// <summary>
    /// Attribute that allows assembly scanning to detect and ignore the type, in which this attribute resides
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AssemblyScanIgnoreAttribute : Attribute 
    {
        /// <summary>
        /// A short description for the reasoning behind why the attribute is used. Only used for documentation reasons
        /// </summary>
        public AssemblyScanIgnoreAttribute(string reason) { }
    }
}
