using BladeRazor.Attributes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BladeRazor.TagHelpers
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
                if (d.GetAutoGenerateField() != null)
                    if (!d.AutoGenerateField)
                        return false;
            }

            return true;
        }

        // TODO: Render according to format string attribute too        
        public static string GetFormattedValue(ModelExplorer explorer, DataTypeAttribute dta, bool renderHtml)
        {
            HtmlContentBuilder content = new HtmlContentBuilder();

            if (explorer.Model == null)
                return string.Empty;

            var value = explorer.Model.ToString();
            content.SetContent(value);

            if (dta == null)
                return value;

            //TODO: Implement DataTypes as and when these are required
            value = dta.DataType switch
            {
                DataType.Custom => value,
                DataType.DateTime => FormatDateTime(explorer, dta, value),
                DataType.Date => FormatDateTime(explorer, dta, value),
                DataType.Time => FormatDateTime(explorer, dta, value),
                DataType.Duration => value,
                DataType.PhoneNumber => value,
                DataType.Currency => value,
                DataType.Text => value,
                DataType.Html => value,
                DataType.MultilineText => value,
                DataType.EmailAddress => value,
                DataType.Password => value,
                DataType.Url => value,
                DataType.ImageUrl => value,
                DataType.CreditCard => value,
                DataType.PostalCode => value,
                DataType.Upload => value,
                _ => value
            };
            content.SetContent(value);
            return value;
        }

        private static string FormatEmail(ModelExplorer explorer, DataTypeAttribute dta, string value, bool renderHtml)
        {
            if (!renderHtml)
                return value;
            if (dta == null || explorer.Model == null)
                return value;

            return value;
            //TagBuilder tag = new TagBuilder("a");
            //tag.TagRenderMode = TagRenderMode.Normal;
            //tag.Attributes.Add("href", $"mailto:{value}");
            //tag.InnerHtml.Append(value);
            //return tag;

        }

        private static string FormatDateTime(ModelExplorer explorer, DataTypeAttribute dta, string value)
        {
            if (dta == null || explorer.Model == null)
                return value;
            if (explorer.Model.GetType() != typeof(DateTime))
                return value;

            if (dta.DataType == DataType.Date)
                return ((DateTime)explorer.Model).ToShortDateString();
            if (dta.DataType == DataType.Time)
                return ((DateTime)explorer.Model).ToShortTimeString();

            return value;
        }

        public static string GetComplexValue(ModelExplorer explorer, FormAttribute fa, string value, bool renderHtml)
        {
            if (explorer.Metadata.IsComplexType && fa != null && !string.IsNullOrWhiteSpace(fa.ComplexDisplayProperty))
            {
                var pp = explorer.Properties.Where(pp => pp.Metadata.PropertyName == fa.ComplexDisplayProperty).FirstOrDefault();
                if (pp != null)
                {
                    var dta = GetAttribute<DataTypeAttribute>(pp.Metadata);
                    value = GetFormattedValue(pp, dta, renderHtml);
                }
            }

            return value;
        }
    }
}
