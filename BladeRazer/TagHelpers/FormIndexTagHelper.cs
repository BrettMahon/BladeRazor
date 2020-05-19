using BladeRazer.Attributes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace BladeRazer.TagHelpers
{
    [HtmlTargetElement("form-index", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormIndexTagHelper : FormBaseTagHelper
    {
        public string TableCssClass { get; set; } = "table table-hover table-responsive w-100 d-block d-md-table";

        // TODO: make this an attribute field - this hides on mobile
        private string hideColumnMobileClass = "d-none d-sm-table-cell";

        /// <summary>
        /// Set empty to hide edit button
        /// </summary>
        public string EditPage { get; set; } = "Edit";
        /// <summary>
        /// Set empty to hide view button
        /// </summary>
        public string ViewPage { get; set; } = "View";
        /// <summary>
        /// Set empty to hide delete button
        /// </summary>
        public string DeletePage { get; set; } = "Delete";

        public string EditCommand { get; set; } = "edit";
        public string ViewCommand { get; set; } = "view";
        public string DeleteCommand { get; set; } = "delete";

        public bool UseCommands { get; set; } = false;

        /// <summary>
        /// Comma seperated
        /// </summary>
        public string HideProperties { get; set; }


        public FormIndexTagHelper(IHtmlGenerator generator) : base(generator) { }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            var hideProperties = HideProperties?.Split(',').Select(p => p.Trim()).ToList();
            
            output.TagName = "table";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", TableCssClass);

            // check that we have a list
            if (!For.Metadata.IsCollectionType)
                return;

            // create headers
            string keyProperty = null;
            var headerRow = new TagBuilder("tr") { TagRenderMode = TagRenderMode.Normal };
            foreach (var p in For.Metadata.ElementMetadata.Properties)
            {
                if (hideProperties != null && hideProperties.Contains(p.PropertyName))
                    continue;
                var formIndexAttribute = Utility.GetAttribute<FormIndexAttribute>(p);
                var formAttribute = Utility.GetAttribute<FormAttribute>(p);

                // test for key
                var keyAttribute = Utility.GetAttribute<KeyAttribute>(p);
                if (keyAttribute != null)
                    keyProperty = p.Name;

                // do not display if either attribute hides it
                if (formAttribute?.Type == FormInputType.Hidden)
                    continue;
                if (formIndexAttribute?.Hidden == true)
                    continue;

                // render the cell
                var headerCell = new TagBuilder("th");
                //TODO: uncomment in order to hide this on mobile
                //headerCell.Attributes.Add("class", hideColumnMobileClass);
                if (p.DisplayName != null)
                    headerCell.InnerHtml.AppendHtml(p.DisplayName);
                else
                    headerCell.InnerHtml.Append(p.Name);
                headerRow.InnerHtml.AppendHtml(headerCell);
            }

            // if we do not have a key search for a property called Id
            if (string.IsNullOrWhiteSpace(keyProperty))
                keyProperty = For.Metadata.ElementMetadata.Properties.Where(p => p.Name.ToLower() == "id").FirstOrDefault()?.Name;

            // render one last cell for the buttons
            var lastHeader = new TagBuilder("th");
            headerRow.InnerHtml.AppendHtml(lastHeader);
            output.Content.AppendHtml(headerRow);

            // loop through items      
            ICollection collection = (ICollection)For.Model;
            foreach (var item in collection)
            {
               

                // create the row
                var row = new TagBuilder("tr") { TagRenderMode = TagRenderMode.Normal };
                
                // get the explorer
                var explorer = For.ModelExplorer.GetExplorerForModel(item);
                
                // get the key value
                string keyValue = null;
                if (keyProperty != null)
                    keyValue = explorer.Properties.Where(p => p.Metadata.Name == keyProperty).FirstOrDefault()?.Model.ToString();

                // loop through the element properties
                foreach (var p in explorer.Properties)
                {
                    if (hideProperties != null && hideProperties.Contains(p.Metadata.PropertyName))
                        continue;

                    var formIndexAttribute = Utility.GetAttribute<FormIndexAttribute>(p.Metadata);
                    var formAttribute = Utility.GetAttribute<FormAttribute>(p.Metadata);
                    var dataAttribute = Utility.GetAttribute<DataTypeAttribute>(p.Metadata);

                    // do not display if either attribute hides it
                    if (formAttribute?.Type == FormInputType.Hidden)
                        continue;
                    if (formIndexAttribute?.Hidden == true)
                        continue;

                    string value = p.Model?.ToString() ?? string.Empty;

                    // check for date attribute 
                    // TODO: this can be extended
                    // TODO: render according to format string attribute too
                    // TODO: perform this check on complex types too - will require this to go into a method
                    if (dataAttribute != null && p.Model != null)
                    {
                        if (p.Model.GetType() == typeof(DateTime))
                        {
                            if (dataAttribute.DataType == DataType.Date)
                                value = ((DateTime)p.Model).ToShortDateString();
                            if (dataAttribute.DataType == DataType.Time)
                                value = ((DateTime)p.Model).ToShortTimeString();
                        }
                    }

                    // check for complex object
                    if (p.Metadata.IsComplexType && formIndexAttribute != null)
                    {
                        var displayProperty = formIndexAttribute.DisplayProperty;
                        if (displayProperty != null)
                        {
                            var pp = p.Properties.Where(pp => pp.Metadata.PropertyName == displayProperty).FirstOrDefault();
                            if (pp != null)
                                value = pp.Model?.ToString() ?? string.Empty;
                        }
                    }

                    // render the cell
                    var cell = new TagBuilder("td");
                    //TODO: uncomment in order to hide this on mobile
                    //cell.Attributes.Add("class", hideColumnMobileClass);
                    cell.InnerHtml.Append(value);
                    row.InnerHtml.AppendHtml(cell);
                }

                // render the buttons cell
                if (keyValue != null)
                {
                    var buttons = new TagBuilder("td");                    
                    var routes = new Dictionary<string, string>() { { keyProperty.ToLower(), keyValue } };
                    if (!string.IsNullOrEmpty(ViewPage))
                        buttons.InnerHtml.AppendHtml(GenerateViewButton(explorer, keyProperty, keyValue));
                    if (!string.IsNullOrEmpty(EditPage))
                        buttons.InnerHtml.AppendHtml(GenerateEditButton(explorer, keyProperty, keyValue));
                    if (!string.IsNullOrEmpty(DeletePage))
                        buttons.InnerHtml.AppendHtml(GenerateDeleteButton(explorer, keyProperty, keyValue));
                    row.InnerHtml.AppendHtml(buttons);
                }
                output.Content.AppendHtml(row);
            }
        }


        protected virtual IHtmlContent GenerateViewButton(ModelExplorer itemExplorer, string keyProperty, string keyValue)
        {
            var routes = new Dictionary<string, string>()
            {
                { keyProperty.ToLower(), keyValue }                
            };
            if (UseCommands)
                routes.Add("command", ViewCommand);
            
            return GenerateAnchorTagHelper(ViewPage, "View", "btn btn-primary m-1", routes);
        }

        protected virtual IHtmlContent GenerateEditButton(ModelExplorer itemExplorer, string keyProperty, string keyValue)
        {
            var routes = new Dictionary<string, string>()
            {
                { keyProperty.ToLower(), keyValue }
            };

            if (UseCommands)
                routes.Add("command", EditCommand);

            return GenerateAnchorTagHelper(EditPage, "Edit", "btn btn-info m-1", routes);
        }

        protected virtual IHtmlContent GenerateDeleteButton(ModelExplorer itemExplorer, string keyProperty, string keyValue)
        {
            var routes =  new Dictionary<string, string>() 
            { 
                { keyProperty.ToLower(), keyValue }
            };

            if (UseCommands)
                routes.Add("command", DeleteCommand);

            return GenerateAnchorTagHelper(DeletePage, "Delete", "btn btn-danger m-1", routes);
        }      


    }

}
