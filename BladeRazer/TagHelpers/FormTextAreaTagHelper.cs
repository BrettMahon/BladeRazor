using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeRazor.TagHelpers
{
    [HtmlTargetElement("form-textarea", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormTextAreaTagHelper : FormBaseTagHelper
    {
        [HtmlAttributeName("asp-rows")]
        public int Rows { get; set; } = 2;

        public FormTextAreaTagHelper(IHtmlGenerator generator, IStyles styles = null) : base(generator, styles) { }

        protected override TagHelperOutput GenerateTagHelper() =>
            tg.GenerateTextAreaTagHelper(For, Rows);
    }
}
