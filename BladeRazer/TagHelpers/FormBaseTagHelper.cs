using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public abstract class FormBaseTagHelper : TagHelper
    {
        protected IHtmlGenerator generator;
        protected TagGenerator tg;
        protected Styles styles;

        public FormBaseTagHelper(IHtmlGenerator generator)
        {
            this.generator = generator;
            this.tg = new TagGenerator(generator, ViewContext);
            this.styles = new Styles();
        }

        public FormBaseTagHelper(IHtmlGenerator generator, Styles styles)
        {
            this.generator = generator;
            this.tg = new TagGenerator(generator, ViewContext);
            this.styles = styles;
        }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
       
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", styles.FormGroup);

            output.Content.AppendHtml(tg.GenerateLabel(For, Styles.CssLabel));
            output.Content.AppendHtml(GenerateTagHelper());
            output.Content.AppendHtml(tg.GenerateValidation(For, Styles.CssValidation));
        }        

        protected virtual TagHelperOutput GenerateTagHelper()
        {
            throw new NotImplementedException();
        }
    }
}
