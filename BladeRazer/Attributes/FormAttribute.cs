using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace BladeRazer.Attributes
{
    public enum FormInputType
    {
        Auto = 0,
        Hidden = 1,
        Select = 2,
        TextArea = 3
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FormAttribute : Attribute
    {
        public FormInputType Type { get; set; } = FormInputType.Auto;
        public int TextAreaRows { get; set; }
        public string SelectItemsKey { get; set; }
        public string SelectOptionName { get; set; } = null;
        public string SelectOptionValue { get; set; } = string.Empty;

        public FormAttribute(FormInputType type)
        {
            Type = type;
        }
    }

    
}
