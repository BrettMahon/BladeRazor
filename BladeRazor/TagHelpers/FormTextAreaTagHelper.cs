// Copyright (c) Brett Lyle Mahon. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

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
