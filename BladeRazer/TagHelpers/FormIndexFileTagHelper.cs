using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BladeRazer.TagHelpers
{
    [HtmlTargetElement("form-index-file", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormIndexFileTagHelper : FormIndexTagHelper
    {
        [HtmlAttributeName("asp-filepath")]
        public string FilePath { get; set; } = "/uploads/";
        [HtmlAttributeName("asp-filename-property")]
        public string FileNameProperty { get; set; } = "FileName";

        protected bool maintainRoutes = false;
        protected string routeConflictPrefix = "_";

        public FormIndexFileTagHelper(IHtmlGenerator generator, IHtmlHelper htmlHelper, IStyles styles = null) : base(generator, htmlHelper, styles) { }

        protected override IHtmlContent GenerateViewButton(ModelExplorer itemExplorer, string keyProperty, string keyValue)
        {
            // get the file name
            var filename = itemExplorer.Properties.Where(p => p.Metadata.PropertyName == FileNameProperty).FirstOrDefault()?.Model?.ToString();
            if (filename == null) { }

            // build the link
            TagBuilder tagBuilder = new TagBuilder("a");
            tagBuilder.Attributes.Add("href", FilePath + filename);
            tagBuilder.Attributes.Add("target", "_blank");
            tagBuilder.Attributes.Add("class", styles.ButtonView);
            tagBuilder.InnerHtml.Append("View");

            return tagBuilder;
        }

        protected override IHtmlContent GenerateDeleteButton(ModelExplorer itemExplorer, string keyProperty, string keyValue)
        {

            var routes = new Dictionary<string, string>();
            keyProperty = keyProperty.ToLower();
            if (maintainRoutes)
            {
                routes = MaintainRoutes(keyProperty, keyValue);
            }
            else
            {
                routes.Add(keyProperty, keyValue);
                if (CommandsEnabled)
                    routes.Add("command", deleteCommand);
            }
            return tg.GenerateAnchorTagHelper(DeletePage, "Delete", styles.ButtonDelete, routes);
        }

        private Dictionary<string, string> MaintainRoutes(string keyProperty, string keyValue)
        {
            var routes = new Dictionary<string, string>();

            // add the existing routes
            foreach (var q in ViewContext.HttpContext.Request.Query)
                routes.Add(q.Key.ToLower(), q.Value);

            // now we check for conflicts. 
            if (routes.ContainsKey(keyProperty))
                routes.Add($"{routeConflictPrefix}{keyProperty}", keyValue);
            else
                routes.Add(keyProperty, keyValue);

            // now we check for conflicts. 
            if (CommandsEnabled)
            {
                if (routes.ContainsKey("command"))
                    routes.Add($"{routeConflictPrefix}command", deleteCommand);
                else
                    routes.Add("command", deleteCommand);
            }

            return routes;
        }
    }
}
