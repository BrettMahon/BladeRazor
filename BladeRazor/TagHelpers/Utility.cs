using BladeRazor.Attributes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
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


        // we have two versions of this. This one is from the metadata only       
        public static string GetKeyProperty(ModelPropertyCollection properties)
        {
            string keyProperty = null;
            foreach (var p in properties)
            {
                // test for key
                var keyAttribute = Utility.GetAttribute<KeyAttribute>(p);
                if (keyAttribute != null)
                    keyProperty = p.Name;
            }

            // if we do not have a key search for a property called Id
            if (string.IsNullOrWhiteSpace(keyProperty))
                keyProperty = properties.Where(p => p.Name.ToLower() == "id").FirstOrDefault()?.Name;

            return keyProperty;
        }

        // the ModelExplorer version can be used for dynamic types but then you will need a non-null object
        public static string GetKeyProperty(IEnumerable<ModelExplorer> properties)
        {
            string keyProperty = null;
            foreach (var p in properties)
            {
                // test for key
                var keyAttribute = Utility.GetAttribute<KeyAttribute>(p.Metadata);
                if (keyAttribute != null)
                    keyProperty = p.Metadata.PropertyName;
            }

            // if we do not have a key search for a property called Id
            if (string.IsNullOrWhiteSpace(keyProperty))
                keyProperty = properties.Where(p => p.Metadata.PropertyName.ToLower() == "id").FirstOrDefault()?.Metadata.PropertyName;

            return keyProperty;
        }


        public static string GetKeyValue(string keyProperty, ModelExplorer explorer)
        {
            if (keyProperty == null || explorer == null)
                return null;

            return explorer.Properties.Where(p => p.Metadata.Name == keyProperty).FirstOrDefault()?.Model.ToString();
        }


        public static bool DisplayForEdit(ModelMetadata meta)
        {
            // check metadata
            if (meta.ShowForEdit == false)
                return false;

            // read-only fields are skipped
            if (meta.IsReadOnly)
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
            if (p.Model == null)
                return new HtmlContentBuilder().Append(null);

            // check for mail address
            if (renderHtml && p.Metadata.DataTypeName == "EmailAddress")
            {
                var email = p.Model.ToString();
                TagBuilder t = new TagBuilder("a");
                t.Attributes.Add("href", $"mailto:{email}");
                t.TagRenderMode = TagRenderMode.Normal;
                t.InnerHtml.Append(email);
                return t;
            }

            // if we have a boolean - render the checkbox
            if (renderHtml && p.Model.GetType() == typeof(bool))
            {
                var key = Guid.NewGuid().ToString();
                htmlHelper.ViewData.Add(key, p.Model);
                return htmlHelper.Display(key);
            }

            // otherwise simply render the formatted text
            string formattedValue = p.Model.ToString();
            if (!string.IsNullOrWhiteSpace(p.Metadata.DisplayFormatString))
                formattedValue = string.Format(p.Metadata.DisplayFormatString, p.Model);
            return new HtmlContentBuilder().Append(formattedValue);

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
