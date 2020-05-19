﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

// https://coding.abel.nu/2018/04/a-form-entry-tag-helper/

namespace BladeRazer.TagHelpers
{
    [HtmlTargetElement("form-input", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormInputTagHelper : FormBaseTagHelper
    {
        public FormInputTagHelper(IHtmlGenerator generator) : base(generator) { }

        protected override TagBuilder GenerateControl() =>
                generator.GenerateTextBox(ViewContext, For.ModelExplorer, For.Name, For.Model, Format, new { @class = "form-control" });

        protected override TagHelperOutput GenerateTagHelper() => GenerateInputTagHelper(For);
       
    }
}