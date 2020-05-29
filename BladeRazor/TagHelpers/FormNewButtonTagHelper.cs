// Copyright (c) Brett Lyle Mahon. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System;

namespace BladeRazor.TagHelpers
{
    [HtmlTargetElement("form-new-button", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormNewButtonTagHelper : FormBaseTagHelper
    {
        [HtmlAttributeName("asp-page")]
        public string Page { get; set; } = null;

        [HtmlAttributeName("asp-action")]
        public string Action { get; set; } = null;

        [HtmlAttributeName("asp-text")]
        public string Text { get; set; } = "New";

        [HtmlAttributeName("asp-display-icon")]
        public bool DisplayIcon { get; set; } = false;

        [HtmlAttributeName("asp-route-id")]
        public int RouteId { get; set; } = 0;

        public FormNewButtonTagHelper(IHtmlGenerator generator, IStyles styles = null) : base(generator, styles) { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var routes = new Dictionary<string, string>();
            if (RouteId != 0)
                routes.Add("id", RouteId.ToString());

            TagHelperOutput a = null;
            if (!string.IsNullOrWhiteSpace(Page))
            {
                a = tg.GenerateAnchorTagHelper(Page, null, styles.ButtonNew, routes);
            }
            else
            {
                a = tg.GenerateAnchorActionTagHelper(Action, null, styles.ButtonNew, routes);
            }

            var plus = new TagBuilder("span");
            plus.TagRenderMode = TagRenderMode.Normal;
            if (DisplayIcon)
            {
                plus.Attributes.Add("class", styles.ButtonNewIcon);
                a.Content.AppendHtml(plus);
            }
            a.Content.AppendHtml($" {Text}");
            output.Content.AppendHtml(a);
        }
    }
}
