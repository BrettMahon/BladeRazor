﻿// Copyright (c) Brett Lyle Mahon. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BladeRazor.TagHelpers
{
    [HtmlTargetElement("form-submit", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormSubmitTagHelper : FormBaseTagHelper
    {
        [HtmlAttributeName("asp-submit-text")]
        public string SubmitText { get; set; } = "Save";
        [HtmlAttributeName("asp-submit-class")]
        public string SubmitClass { get; set; } = null;
        [HtmlAttributeName("asp-cancel-text")]
        public string CancelText { get; set; } = "Back";
        [HtmlAttributeName("asp-cancel-class")]
        public string CancelClass { get; set; } = null;
        [HtmlAttributeName("asp-cancel-page")]
        public string CancelPage { get; set; } = "Index";
        [HtmlAttributeName("asp-cancel-action")]
        public string CancelAction { get; set; } = null;
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
            if (string.IsNullOrWhiteSpace(SubmitClass))
                submit.Attributes.Add("class", styles.ButtonSubmit);
            else
                submit.Attributes.Add("class", SubmitClass);
            return submit;
        }

        protected virtual IHtmlContent GenerateCancel()
        {
            // deterine style
            string style = styles.ButtonCancel;
            if (!string.IsNullOrWhiteSpace(CancelClass))
                style = CancelClass;

            // normal case - support both page and action
            // default case is page unless you explicitly set action
            if (!JavaScriptBack)
            {
                if (string.IsNullOrWhiteSpace(CancelAction))
                    return tg.GenerateAnchorTagHelper(CancelPage, CancelText, style, null);
                else
                    return tg.GenerateAnchorActionTagHelper(CancelAction, CancelText, style, null);
            }

            var a = new TagBuilder("a");
            a.Attributes.Add("class", style);
            a.Attributes.Add("href", "javascript:history.go(-1)");
            a.InnerHtml.Append(CancelText);
            return a;
        }
    }
}
