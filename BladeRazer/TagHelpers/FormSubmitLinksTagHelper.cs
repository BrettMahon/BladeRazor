using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BladeRazor.TagHelpers
{

    [HtmlTargetElement("form-submit-links", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormSubmitLinksTagHelper : FormSubmitTagHelper
    {
        [HtmlAttributeName("asp-submit-page")]
        public string SubmitPage { get; set; } = "Edit";

        private string keyProperty = null;
        private string keyValue = null;

        public FormSubmitLinksTagHelper(IHtmlGenerator generator, IStyles styles = null) : base(generator, styles) { }

        protected override IHtmlContent GenerateSubmit()
        {
            var routes = new Dictionary<string, string>();
            if (For != null)
            {
                var keyProperty = Utility.GetKeyProperty(For.Metadata.Properties);
                var keyValue = Utility.GetKeyValue(keyProperty, For.ModelExplorer);
             
                if (keyProperty != null && keyValue != null)
                    routes.Add(keyProperty.ToLower(), keyValue);
            }
            return tg.GenerateAnchorTagHelper(SubmitPage, SubmitText, styles.ButtonSubmit, routes);
        }
    }
}
