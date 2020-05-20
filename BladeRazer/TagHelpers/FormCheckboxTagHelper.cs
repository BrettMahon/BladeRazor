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
        public FormCheckboxTagHelper(IHtmlGenerator generator, IStyles styles = null) : base(generator, styles) { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", styles.FormGroup);
            output.Content.AppendHtml(tg.GenerateLabel(For));          
            output.Content.AppendHtml(tg.GenerateCheckboxGroup(For));           
        }
    }
}
