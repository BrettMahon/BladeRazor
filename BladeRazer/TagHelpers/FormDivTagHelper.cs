using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeRazor.TagHelpers 
{
    [HtmlTargetElement("form-div", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormDivTagHelper : FormBaseTagHelper
    {
        public FormDivTagHelper(IHtmlGenerator generator, IStyles styles = null) : base(generator, styles) { }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "row");
            //div
            TagBuilder div = new TagBuilder("div");            
            div.Attributes.Add("class", styles.DivCol);
            // child content
            div.InnerHtml.AppendHtml(await output.GetChildContentAsync());
            // append
            output.Content.AppendHtml(div);
        }
    }
}
