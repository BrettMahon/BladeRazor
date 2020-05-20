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
    // TODO: Also check for built in multi-line DataType property
    [HtmlTargetElement("form-textarea", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormTextAreaTagHelper : FormBaseTagHelper
    {
        public int Rows { get; set; } = 4;

        public FormTextAreaTagHelper(IHtmlGenerator generator) : base(generator) { }
        public FormTextAreaTagHelper(IHtmlGenerator generator, Styles styles) : base(generator, styles) { }

        protected override TagHelperOutput GenerateTagHelper() =>
            tg.GenerateTextAreaTagHelper(For, Rows);
    }
}
