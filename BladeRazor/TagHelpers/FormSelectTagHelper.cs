﻿// Copyright (c) Brett Lyle Mahon. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace BladeRazor.TagHelpers
{
    [HtmlTargetElement("form-select", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormSelectTagHelper : FormBaseTagHelper
    {
        [HtmlAttributeName("asp-items")]
        public IEnumerable<SelectListItem> Items { get; set; }
        [HtmlAttributeName("asp-option-name")]
        public string OptionName { get; set; } = null;
        [HtmlAttributeName("asp-option-value")]
        public string OptionValue { get; set; } = string.Empty;

        //TODO: Implement select child content for option tags inline so we have that too
        public TagHelperContent ChildContent { get; set; }

        public FormSelectTagHelper(IHtmlGenerator generator, IStyles styles = null) : base(generator, styles) { }

        protected override TagHelperOutput GenerateTagHelper() =>
            tg.GenerateSelectTagHelper(For, Items, OptionName, OptionValue);
    }
}
