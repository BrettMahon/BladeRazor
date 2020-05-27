using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BladeRazor.TagHelpers
{
    // TODO: FormValidationTagHelper: This is not yet complete. We have validation on the individual fields. Not sure it adds value
    [HtmlTargetElement("form-validation-summary", TagStructure = TagStructure.NormalOrSelfClosing)]
    class FormValidationSummaryTagHelper : FormBaseTagHelper
    {
        public FormValidationSummaryTagHelper(IHtmlGenerator generator, IStyles styles = null) : base(generator, styles) { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Content.AppendHtml(tg.GenerateValidationSummaryTagHelper());
        }
    }
}
