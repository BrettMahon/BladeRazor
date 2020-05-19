using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeRazer.TagHelpers
{
    // TODO: This is not complete
    [HtmlTargetElement("form-validation-summary", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormValidationSummaryTagHelper : FormBaseTagHelper
    {
        public FormValidationSummaryTagHelper(IHtmlGenerator generator) : base(generator) { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Content.AppendHtml(tg.GenerateValidationSummaryTagHelper());
        }
    }
}
