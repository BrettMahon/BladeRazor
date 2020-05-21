﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeRazer.TagHelpers 
{
    // TODO: FormWrapperTagHelper: This is not implemented yet
    [HtmlTargetElement("form-wrapper", TagStructure = TagStructure.NormalOrSelfClosing)]
    class FormWrapperTagHelper : FormBaseTagHelper
    {
        public FormWrapperTagHelper(IHtmlGenerator generator, IStyles styles = null) : base(generator, styles) { }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", styles.DivRow);
            //div
            TagBuilder div = new TagBuilder("div");            
            div.Attributes.Add("class", styles.DivCol);            
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
