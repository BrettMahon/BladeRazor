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
    [HtmlTargetElement("form-select", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormSelectTagHelper : FormBaseTagHelper
    {
        [HtmlAttributeName("asp-items")]
        public IEnumerable<SelectListItem> Items { get; set; }
        [HtmlAttributeName("asp-option-name")]
        public string OptionName { get; set; } = null;        
        [HtmlAttributeName("asp-option-value")]
        public string OptionValue { get; set; } = string.Empty;

        //TODO: implement child content for option tags inline so we have that option too
        public TagHelperContent ChildContent { get; set; }

        public FormSelectTagHelper(IHtmlGenerator generator) : base(generator) { }

        protected override TagHelperOutput GenerateTagHelper() => 
            GenerateSelectTagHelper(For, Items, OptionName, OptionValue);
    }
}
