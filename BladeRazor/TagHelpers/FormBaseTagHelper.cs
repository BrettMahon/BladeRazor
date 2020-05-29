// Copyright (c) Brett Lyle Mahon. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace BladeRazor.TagHelpers
{
    public abstract class FormBaseTagHelper : TagHelper
    {
        protected IHtmlGenerator generator;
        protected TagGenerator tg;
        protected IStyles styles;

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }


        public FormBaseTagHelper()
        { }

        public FormBaseTagHelper(IHtmlGenerator generator, IStyles styles = null)
        {
            this.generator = generator;
            this.styles = styles ?? new Styles();
        }

        public override void Init(TagHelperContext context)
        {
            this.tg = new TagGenerator(generator, ViewContext);
            base.Init(context);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", styles.FormGroup);

            output.Content.AppendHtml(tg.GenerateLabel(For));
            output.Content.AppendHtml(GenerateTagHelper());
            output.Content.AppendHtml(tg.GenerateValidation(For));
        }

        protected virtual TagHelperOutput GenerateTagHelper()
        {
            throw new NotImplementedException();
        }
    }
}
