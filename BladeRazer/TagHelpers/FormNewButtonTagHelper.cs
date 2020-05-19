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

        private string cssClass = "btn btn-success m-1";

        public FormNewButtonTagHelper(IHtmlGenerator generator) : base(generator) { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var routes = new Dictionary<string, string>()
            {
                { "id", RouteId.ToString() }                
            };

            var a = GenerateAnchorTagHelper(Page, null, cssClass, routes);
            var plus = new TagBuilder("span");
            plus.TagRenderMode = TagRenderMode.Normal;
            plus.Attributes.Add("class", "oi oi-plus");
            a.Content.AppendHtml(plus);
            a.Content.AppendHtml($" {Text}");
            output.Content.AppendHtml(a);
        }
    }
}
