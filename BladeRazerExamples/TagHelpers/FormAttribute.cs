using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeRazer.TagHelpers
{
    public enum InputType
    {
        Default = 0,
        Hidden = 1, 
        Select = 2, 
        TextArea = 3
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FormAttribute : Attribute
    {
        public InputType Type { get; set; } = InputType.Default;
        public int TextAreaRows { get; set; }
        public string SelectItemsKey { get; set; }
        public string SelectOptionName { get; set; } = null;
        public string SelectOptionValue { get; set; } = string.Empty;

        public FormAttribute(InputType type)
        {
            Type = type;
        }
    }

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
