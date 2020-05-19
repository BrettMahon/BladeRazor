﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeRazer.TagHelpers
{
    // TODO: Also check for built in multi-line DataType property
    [HtmlTargetElement("form-textarea", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormTextAreaTagHelper : FormBaseTagHelper
    {
        public int Rows { get; set; } = 4;

        public FormTextAreaTagHelper(IHtmlGenerator generator) : base(generator) { }

        protected override TagBuilder GenerateControl() =>
                generator.GenerateTextArea(ViewContext, For.ModelExplorer, For.Name, Rows, 0, new { @class = "form-control" });

        protected override TagHelperOutput GenerateTagHelper() =>
            GenerateTextAreaTagHelper(For, Rows);
    }
}