// Copyright (c) Brett Lyle Mahon. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BladeRazor.TagHelpers
{
    [HtmlTargetElement("form-hidden", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormHiddenTagHelper : FormBaseTagHelper
    {
        public FormHiddenTagHelper(IHtmlGenerator generator, IStyles styles = null) : base(generator, styles) { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            
            output.Content.AppendHtml(GenerateTagHelper());
        }

        protected override TagHelperOutput GenerateTagHelper() => tg.GenerateHiddenTagHelper(For);

    }
}
