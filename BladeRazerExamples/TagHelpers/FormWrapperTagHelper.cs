using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeRazer.TagHelpers 
{
    [HtmlTargetElement("form-wrapper", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormWrapperTagHelper : FormBaseTagHelper
    {
        public string ColClass { get; set; } = "col-md-4";

        public FormWrapperTagHelper(IHtmlGenerator generator) : base(generator) { }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "row");
            //div
            TagBuilder div = new TagBuilder("div");            
            div.Attributes.Add("class", ColClass);            
            // form
            TagBuilder form = new TagBuilder("form");
            form.Attributes.Add("method", "post");        
            // child content
            form.InnerHtml.AppendHtml(await output.GetChildContentAsync());
            // append
            div.InnerHtml.AppendHtml(form);
            output.Content.AppendHtml(div);
        }
    }
}
