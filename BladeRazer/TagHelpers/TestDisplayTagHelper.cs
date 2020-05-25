using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeRazor.TagHelpers
{
    
    // TODO: Remove this
    public class TestDisplayTagHelper : TagHelper
    {
        protected IHtmlGenerator generator;
        protected IHtmlHelper htmlHelper;

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public TestDisplayTagHelper(IHtmlGenerator generator, IHtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
            this.generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "form-group");

            if (htmlHelper is IViewContextAware)
                ((IViewContextAware)htmlHelper).Contextualize(ViewContext);
            var content = htmlHelper.Display(For.Name);


          
            output.Content.AppendHtml(content);
           
        }

    }
}
