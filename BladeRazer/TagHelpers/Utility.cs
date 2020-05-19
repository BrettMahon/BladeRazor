using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BladeRazer.TagHelpers
{
    class Utility
    {
        public static T GetAttribute<T>(ModelMetadata p) where T : Attribute
        {
            T attribute = null;
            if (p is Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.DefaultModelMetadata meta)
                attribute = (T)meta.Attributes.PropertyAttributes.Where(p => p.GetType() == typeof(T)).FirstOrDefault();
            return attribute;
        }
    }
}
