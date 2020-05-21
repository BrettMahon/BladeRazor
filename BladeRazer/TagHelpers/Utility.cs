using BladeRazer.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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


        /// <summary>
        /// Determine whether we should display this in the view taghelpers (index and details)
        /// </summary>        
        public static bool DisplayView(Attribute a)
        {
            if (a == null)
                return true;

            if (a is FormAttribute f)
            {
                if (f.Type == FormInputType.Hidden || f.Type == FormInputType.None)
                    return false;
                if (!f.DisplayView)
                    return false;
            }

            if (a is DisplayAttribute d)
            {
                if (d.AutoGenerateField)
                    return false;
            }

            return true;
        }
        
        public static string GetFormattedValue(ModelExplorer explorer, DataTypeAttribute dta)
        {
            string value = explorer.Model?.ToString() ?? string.Empty;
            // TODO: Render according to format string attribute too
            // TODO: Support the DisplayFormat built in attribute
            // TODO: Support additional data types here too
            if (dta != null && explorer.Model != null)
            {
                if (explorer.Model.GetType() == typeof(DateTime))
                {
                    if (dta.DataType == DataType.Date)
                        value = ((DateTime)explorer.Model).ToShortDateString();
                    if (dta.DataType == DataType.Time)
                        value = ((DateTime)explorer.Model).ToShortTimeString();
                }
            }

            return value;
        }

        public static string GetComplexValue(ModelExplorer explorer, FormAttribute fa, string value)
        {
            if (explorer.Metadata.IsComplexType && fa != null && !string.IsNullOrWhiteSpace(fa.DisplayProperty))
            {
                var pp = explorer.Properties.Where(pp => pp.Metadata.PropertyName == fa.DisplayProperty).FirstOrDefault();
                if (pp != null)
                {
                    var dta = GetAttribute<DataTypeAttribute>(pp.Metadata);
                    value = GetFormattedValue(pp, dta);
                }
            }

            return value;
        }
    }
}
