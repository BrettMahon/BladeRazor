using System;
using System.Collections.Generic;
using System.Text;

namespace BladeRazer.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FormIndexAttribute : Attribute
    {
        public string DisplayProperty { get; set; }
        public bool Hidden { get; set; } = false;

        public FormIndexAttribute()
        {
        }
    }
}
