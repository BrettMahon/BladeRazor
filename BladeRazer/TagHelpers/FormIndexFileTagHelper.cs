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
        public string FilePath { get; set; } = "/uploads/";
        public string FileNameProperty { get; set; } = "FileName";
        /// <summary>
        /// If set will maintain the existing query string and append
        /// </summary>
        public bool MaintainRoutes = false;
        public string RouteConflictPrefix = "_";

        public FormIndexFileTagHelper(IHtmlGenerator generator) : base(generator) { }
        public FormIndexFileTagHelper(IHtmlGenerator generator, Styles styles) : base(generator, styles) { }

        protected override IHtmlContent GenerateViewButton(ModelExplorer itemExplorer, string keyProperty, string keyValue)
        {
            // get the file name
            var filename = itemExplorer.Properties.Where(p => p.Metadata.PropertyName == FileNameProperty).FirstOrDefault()?.Model?.ToString();
            if (filename == null) { }

            // build the link
            TagBuilder tagBuilder = new TagBuilder("a");
            tagBuilder.Attributes.Add("href", FilePath + filename);
            tagBuilder.Attributes.Add("target", "_blank");
            tagBuilder.Attributes.Add("class", "btn btn-primary m-1");
            tagBuilder.InnerHtml.Append("View");


            return tagBuilder;
        }

        protected override IHtmlContent GenerateDeleteButton(ModelExplorer itemExplorer, string keyProperty, string keyValue)
        {
            var routes = new Dictionary<string, string>();

            keyProperty = keyProperty.ToLower();
            if (MaintainRoutes)
            {
                // add the existing routes
                foreach (var q in ViewContext.HttpContext.Request.Query)
                    routes.Add(q.Key.ToLower(), q.Value);
                
                // now we check for conflicts. 
                if (routes.ContainsKey(keyProperty))
                    routes.Add($"{RouteConflictPrefix}{keyProperty}", keyValue);
                else
                    routes.Add(keyProperty, keyValue);

                // now we check for conflicts. 
                if (UseCommands)
                {
                    if (routes.ContainsKey("command"))
                        routes.Add($"{RouteConflictPrefix}command", DeleteCommand);
                    else
                        routes.Add("command", DeleteCommand);
                }
            }
            else
            {
                routes.Add(keyProperty, keyValue);
                if (UseCommands)
                    routes.Add("command", DeleteCommand);
            }
            
            
           
            return tg.GenerateAnchorTagHelper(DeletePage, "Delete", "btn btn-danger m-1", routes);
        }
    }
}
