// Copyright (c) Brett Lyle Mahon. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BladeRazor.TagHelpers
{
    [HtmlTargetElement("form-input", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormInputTagHelper : FormBaseTagHelper
    {
        public FormInputTagHelper(IHtmlGenerator generator, IStyles styles = null) : base(generator, styles) { }

        protected override TagHelperOutput GenerateTagHelper() => tg.GenerateInputTagHelper(For);

    }
}
