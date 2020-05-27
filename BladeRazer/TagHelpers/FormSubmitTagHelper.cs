using Microsoft.AspNetCore.Html;
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
    [HtmlTargetElement("form-submit", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormSubmitTagHelper : FormBaseTagHelper
    {
        [HtmlAttributeName("asp-submit-text")]
        public string SubmitText { get; set; } = "Save";
        [HtmlAttributeName("asp-cancel-text")]
        public string CancelText { get; set; } = "Back";
        [HtmlAttributeName("asp-cancel-page")]
        public string CancelPage { get; set; } = "Index";
        [HtmlAttributeName("asp-javascript-back")]
        public bool JavaScriptBack { get; set; } = false;

        public FormSubmitTagHelper(IHtmlGenerator generator, IStyles styles = null) : base(generator, styles) { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", styles.FormGroup);

            // generate the submit    
            if (!string.IsNullOrWhiteSpace(SubmitText))
                output.Content.AppendHtml(GenerateSubmit());

            // generate the cancel
            if (!string.IsNullOrWhiteSpace(CancelText))
                output.Content.AppendHtml(GenerateCancel());
        }

        protected virtual IHtmlContent GenerateSubmit()
        {
            TagBuilder submit = new TagBuilder("input") { TagRenderMode = TagRenderMode.StartTag };
            submit.Attributes.Add("type", "submit");
            submit.Attributes.Add("value", SubmitText);
            submit.Attributes.Add("class", styles.ButtonSubmit);
            return submit;
        }

        protected virtual IHtmlContent GenerateCancel()
        {
            if (!JavaScriptBack)
                return tg.GenerateAnchorTagHelper(CancelPage, CancelText, styles.ButtonCancel, null);
            
            var a = new TagBuilder("a");
            a.Attributes.Add("class", styles.ButtonCancel);
            a.Attributes.Add("href", "javascript:history.go(-1)");
            a.InnerHtml.Append(CancelText);
            return a;
        }
    }
}
