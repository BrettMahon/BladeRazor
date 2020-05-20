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
    [HtmlTargetElement("form-submit", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormSubmitTagHelper : FormBaseTagHelper
    {
        [HtmlAttributeName("asp-submit-text")]
        public string SubmitText { get; set; } = "Save";
        [HtmlAttributeName("asp-submit-class")]
        public string SubmitClass { get; set; } = "btn btn-info mr-1";
        [HtmlAttributeName("asp-submit-page-handler")]
        public string SubmitPageHandler { get; set; } = "btn btn-info mr-1";
        [HtmlAttributeName("asp-cancel-text")]
        public string CancelText { get; set; } = "Back to List";
        [HtmlAttributeName("asp-cancel-class")]
        public string CancelClass { get; set; } = "btn btn-primary";
        [HtmlAttributeName("asp-cancel-page")]
        public string CancelPage { get; set; } = "Index";
        [HtmlAttributeName("asp-javascript-back")]
        public bool JavaScriptBack { get; set; } = false;

        public FormSubmitTagHelper(IHtmlGenerator generator) : base(generator) { }
        public FormSubmitTagHelper(IHtmlGenerator generator, Styles styles) : base(generator, styles) { }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "form-group");

            // generate the submit
            TagBuilder submit = new TagBuilder("input") { TagRenderMode = TagRenderMode.StartTag };
            submit.Attributes.Add("type", "submit");
            submit.Attributes.Add("value", SubmitText);
            submit.Attributes.Add("class", SubmitClass);
            output.Content.AppendHtml(submit);

            // generate the cancel
            if (string.IsNullOrWhiteSpace(CancelText))
                return;

            // generate the anchor cancel
            if (!JavaScriptBack)
            {
                output.Content.AppendHtml(tg.GenerateAnchorTagHelper(CancelPage, CancelText, CancelClass, null));
            }
            else
            {
                var a = new TagBuilder("a");
                a.Attributes.Add("class", CancelClass);
                a.Attributes.Add("href", "javascript:history.go(-1)");
                a.InnerHtml.Append(CancelText);
                output.Content.AppendHtml(a);
            }
        }
    }
}
