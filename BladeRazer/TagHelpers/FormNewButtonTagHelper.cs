using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeRazer.TagHelpers
{
    [HtmlTargetElement("form-new-button", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormNewButtonTagHelper : FormBaseTagHelper
    {
        [HtmlAttributeName("asp-route-id")]
        public int RouteId { get; set; }

        [HtmlAttributeName("asp-page")]
        public string Page { get; set; }

        [HtmlAttributeName("asp-text")]
        public string Text { get; set; } = "New";

        public FormNewButtonTagHelper(IHtmlGenerator generator) : base(generator) { }
        public FormNewButtonTagHelper(IHtmlGenerator generator, Styles styles) : base(generator, styles) { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var routes = new Dictionary<string, string>()
            {
                { "id", RouteId.ToString() }                
            };

            var a = tg.GenerateAnchorTagHelper(Page, null, styles.ButtonNew, routes);
            var plus = new TagBuilder("span");
            plus.TagRenderMode = TagRenderMode.Normal;
            plus.Attributes.Add("class", styles.ButtonNewIcon);
            a.Content.AppendHtml(plus);
            a.Content.AppendHtml($" {Text}");
            output.Content.AppendHtml(a);
        }
    }
}
