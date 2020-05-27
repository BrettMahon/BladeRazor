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
        protected IHtmlHelper htmlHelper;

        [HtmlAttributeName("asp-render-value-html")]
        public bool RenderValueHtml { get; set; } = true;


        public FormDetailsTagHelper(IHtmlGenerator generator, IHtmlHelper htmlHelper, IStyles styles = null) : base(generator, styles)
        {
            this.htmlHelper = htmlHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            var dl = new TagBuilder("dl");
            dl.TagRenderMode = TagRenderMode.Normal;
            dl.Attributes.Add("class", styles.DescriptionList);
            output.Content.AppendHtml(dl);

            // can we conextualise the htmlhelper - if not do not render html values            
            if (htmlHelper is IViewContextAware ht)
                ht.Contextualize(ViewContext);
            else
                RenderValueHtml = false;

            // loop through the properties
            foreach (var p in For.ModelExplorer.Properties)
            {
                if (p.Metadata.IsCollectionType)
                    continue;

                // check display
                if (!Utility.DisplayForView(p.Metadata))
                    continue;
                
                // set the name
                string name = p.Metadata.DisplayName;
                if (string.IsNullOrWhiteSpace(name))
                    name = p.Metadata.PropertyName;

                // get the content
                var value = Utility.GetFormattedHtml(p, ViewContext, htmlHelper, RenderValueHtml);

                // check for complex object and set value
                value = Utility.GetComplexValue(p, value, ViewContext, htmlHelper, RenderValueHtml);

                // render 
                var dt = new TagBuilder("dt");
                dt.Attributes.Add("class", styles.DefinitionTerm);
                dt.InnerHtml.Append(name);
                var dd = new TagBuilder("dd");
                dd.Attributes.Add("class", styles.DefinitionDescription);
                dd.InnerHtml.AppendHtml(value);

                // add to list
                dl.InnerHtml.AppendHtml(dt);
                dl.InnerHtml.AppendHtml(dd);
            }
        }

        
    }
}
