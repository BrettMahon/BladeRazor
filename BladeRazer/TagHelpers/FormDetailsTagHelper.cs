using BladeRazor.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BladeRazor.TagHelpers
{
    [HtmlTargetElement("form-details", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormDetailsTagHelper : FormBaseTagHelper
    {
        [HtmlAttributeName("asp-cell-value-html")]
        public bool RenderCellHtml { get; set; } = true;

        public FormDetailsTagHelper(IHtmlGenerator generator, IStyles styles = null) : base(generator, styles) { }

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

                // check display
                var fa = Utility.GetAttribute<FormAttribute>(explorer.Metadata);
                var da = Utility.GetAttribute<DisplayAttribute>(explorer.Metadata);
                if (!Utility.DisplayView(fa) || !Utility.DisplayView(da))
                    continue;

                // set the formatted value                
                var dta = Utility.GetAttribute<DataTypeAttribute>(explorer.Metadata);
                var value = Utility.GetFormattedValue(explorer, dta, RenderCellHtml);

                // check for complex object and set value
                value = Utility.GetComplexValue(explorer, fa, value, RenderCellHtml);

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
