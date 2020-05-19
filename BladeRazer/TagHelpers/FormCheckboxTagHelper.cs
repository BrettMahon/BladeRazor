using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeRazer.TagHelpers
{
    [HtmlTargetElement("form-checkbox", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormCheckboxTagHelper : FormBaseTagHelper
    {
        public FormCheckboxTagHelper(IHtmlGenerator generator) : base(generator) { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "form-group");
            output.Content.AppendHtml(GenerateLabel(For));          
            output.Content.AppendHtml(GenerateCheckboxGroup(For));           
        }
    }
}
