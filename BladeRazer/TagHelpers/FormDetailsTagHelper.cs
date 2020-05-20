using BladeRazer.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BladeRazer.TagHelpers
{
    [HtmlTargetElement("form-details", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormDetailsTagHelper : FormBaseTagHelper
    {
        public FormDetailsTagHelper(IHtmlGenerator generator) : base(generator) { }
        public FormDetailsTagHelper(IHtmlGenerator generator, Styles styles) : base(generator, styles) { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "dl";
            output.Attributes.Add("class", styles.DescriptionList);
            output.TagMode = TagMode.StartTagAndEndTag;

            // loop through the properties
            foreach (var explorer in For.ModelExplorer.Properties)
            {
                if (explorer.Metadata.IsCollectionType)
                    continue;

                if (explorer.Metadata.IsReadOnly)
                    continue;

                // set the name
                string name = explorer.Metadata.DisplayName;
                if (string.IsNullOrWhiteSpace(name))
                    name = explorer.Metadata.PropertyName;
                
                // TODO: We are using the display property from the Index attribute. This should be generalised
                var formIndexAttribute = Utility.GetAttribute<FormIndexAttribute>(explorer.Metadata);
                var formAttribute = Utility.GetAttribute<FormAttribute>(explorer.Metadata);

                // do not display if either attribute hides it
                if (formAttribute?.Type == FormInputType.Hidden)
                    continue;
                if (formIndexAttribute?.Hidden == true)
                    continue;

                var dataAttribute = Utility.GetAttribute<DataTypeAttribute>(explorer.Metadata);
                
                // set the value
                string value = explorer.Model?.ToString() ?? string.Empty;

                // TODO: This can be extended: Render according to format string attribute too                
                // TODO: Perform this check on complex types too - will require this to go into a method                
                if (dataAttribute != null && explorer.Model != null)
                {
                    if (explorer.Model.GetType() == typeof(DateTime))
                    {
                        if (dataAttribute.DataType == DataType.Date)
                            value = ((DateTime)explorer.Model).ToShortDateString();
                        if (dataAttribute.DataType == DataType.Time)
                            value = ((DateTime)explorer.Model).ToShortTimeString();
                    }
                }

                // check for complex object
                if (explorer.Metadata.IsComplexType && formIndexAttribute != null)
                {
                    var displayProperty = formIndexAttribute.DisplayProperty;
                    if (displayProperty != null)
                    {
                        var pp = explorer.Properties.Where(pp => pp.Metadata.PropertyName == displayProperty).FirstOrDefault();
                        if (pp != null)
                            value = pp.Model?.ToString() ?? string.Empty;
                    }
                }

                // render 
                var dt = new TagBuilder("dt");
                dt.Attributes.Add("class", styles.DefinitionTerm);
                dt.InnerHtml.Append(name);

                var dd = new TagBuilder("dd");
                dd.Attributes.Add("class", styles.DefinitionDescription);
                dd.InnerHtml.Append(value);

                output.Content.AppendHtml(dt);
                output.Content.AppendHtml(dd);
            }
        }
    }
}
