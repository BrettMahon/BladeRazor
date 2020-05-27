using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BladeRazor.TagHelpers
{
    [HtmlTargetElement("form-hidden", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormHiddenTagHelper : FormBaseTagHelper
    {
        public FormHiddenTagHelper(IHtmlGenerator generator, IStyles styles = null) : base(generator, styles) { }

        protected override TagHelperOutput GenerateTagHelper() => tg.GenerateHiddenTagHelper(For);

    }
}
