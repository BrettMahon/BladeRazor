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

        public static bool DisplayForView(ModelMetadata p)
        {
            // check meta-data
            if (p.ShowForDisplay == false)
                return false;
            
            // check form attribute
            var fa = Utility.GetAttribute<FormAttribute>(p);
            if (fa != null)
            {
                if (fa.Type == FormInputType.Hidden || fa.Type == FormInputType.None)
                    return false;
                if (!fa.DisplayView)
                    return false;
            }

            // check display attribute
            var da = Utility.GetAttribute<DisplayAttribute>(p);
            if (da != null && da.GetAutoGenerateField() != null)
            {
                if (!da.AutoGenerateField)
                    return false;
            }

            // if we made it here then display
            return true;
        }


        public static bool DisplayForEdit(ModelMetadata meta)
        {
            // check metadata
            if (meta.ShowForEdit == false)
                return false;

            // check form attribute
            var fa = Utility.GetAttribute<FormAttribute>(meta);
            if (fa != null)
            {
                if (fa.Type == FormInputType.None)
                    return false;
                if (!fa.DisplayEdit)
                    return false;
            }

            // check display attribute
            var da = Utility.GetAttribute<DisplayAttribute>(meta);
            if (da != null && da.GetAutoGenerateField() != null)
            {
                if (!da.AutoGenerateField)
                    return false;
            }

            // if we made it here then display
            return true;
        }

        public static IHtmlContent GetComplexValue(ModelExplorer explorer, IHtmlContent value, ViewContext viewContext, IHtmlHelper htmlHelper, bool renderHtml)
        {
            // check for complex
            if (!explorer.Metadata.IsComplexType)
                return value;

            // now find the property to display
            var fa = Utility.GetAttribute<FormAttribute>(explorer.Metadata);
            if (fa != null && !string.IsNullOrWhiteSpace(fa.ComplexDisplayProperty))
            {
                var pp = explorer.Properties.Where(pp => pp.Metadata.PropertyName == fa.ComplexDisplayProperty).FirstOrDefault();
                if (pp != null)
                    return GetFormattedHtml(pp, viewContext, htmlHelper, renderHtml);
            }

            // if we got here then just return the value as-is
            return value;           
        }

        public static IHtmlContent GetFormattedHtml(ModelExplorer p, ViewContext viewContext, IHtmlHelper htmlHelper, bool renderHtml)
        {
            // if we have a boolean - render the checkbox
            IHtmlContent value = null;
            if (renderHtml && p.Model.GetType() == typeof(bool))
            {
                htmlHelper.ViewData.Add(p.Metadata.PropertyName, p.Model);
                value = htmlHelper.Display(p.Metadata.PropertyName);
            }
            else
            {
                string formattedValue = p.Model.ToString();
                if (!string.IsNullOrWhiteSpace(p.Metadata.DisplayFormatString))
                    formattedValue = string.Format(p.Metadata.DisplayFormatString, p.Model);
                value = new HtmlContentBuilder().Append(formattedValue);
            }

            return value;
        }

        // this is old 
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


        // this is old
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

        
    }
}
