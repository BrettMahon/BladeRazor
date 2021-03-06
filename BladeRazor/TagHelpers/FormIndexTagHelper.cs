﻿// Copyright (c) Brett Lyle Mahon. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace BladeRazor.TagHelpers
{
    [HtmlTargetElement("form-index", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormIndexTagHelper : FormBaseTagHelper
    {

        protected IHtmlHelper htmlHelper;

        /// <summary>
        /// Set empty to hide buttons
        /// </summary>
        [HtmlAttributeName("asp-edit-page")]
        public string EditPage { get; set; } = "Edit";
        [HtmlAttributeName("asp-view-page")]
        public string ViewPage { get; set; } = "Details";
        [HtmlAttributeName("asp-delete-page")]
        public string DeletePage { get; set; } = "Delete";

        [HtmlAttributeName("asp-commands-enabled")]
        public bool CommandsEnabled { get; set; } = false;
        protected string editCommand = "edit";
        protected string viewCommand = "view";
        protected string deleteCommand = "delete";

        [HtmlAttributeName("asp-render-value-html")]
        public bool RenderValueHtml { get; set; } = true;

        /// <summary>
        /// Comma seperated
        /// </summary>
        [HtmlAttributeName("asp-hide-properties")]
        public string HideProperties { get; set; }

        public FormIndexTagHelper(IHtmlGenerator generator, IHtmlHelper htmlHelper, IStyles styles = null) : base(generator, styles)
        {
            this.htmlHelper = htmlHelper;
        }

        //TODO: Implement Display(Order) built in attribute
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // setup tag
            output.TagName = "table";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", styles.Table);

            // check that we have a list
            if (!For.Metadata.IsCollectionType)
                return;

            // setup properties to hide
            var hideProperties = HideProperties?.Split(',').Select(p => p.Trim()).ToList();

            // get the key property
            string keyProperty = GetKeyProperty();

            // create the headers
            CreateHeaders(output, hideProperties);

            // can we conextualise the htmlhelper - if not do not render html values                  
            if (htmlHelper is IViewContextAware ht)
                ht.Contextualize(ViewContext);
            else
                RenderValueHtml = false;

            // now loop through items      
            var collection = (ICollection)For.Model;
            foreach (var item in collection)
            {
                // create the row
                var row = new TagBuilder("tr") { TagRenderMode = TagRenderMode.Normal };

                // get the explorer
                var explorer = For.ModelExplorer.GetExplorerForModel(item);

                // get the key value               
                string keyValue = Utility.GetKeyValue(keyProperty, explorer);

                // loop through the element properties
                foreach (var p in explorer.Properties)
                {
                    if (hideProperties != null && hideProperties.Contains(p.Metadata.PropertyName))
                        continue;

                    if (!Utility.DisplayForView(p.Metadata))
                        continue;

                    // create a model expression from the explorer
                    //var f = new ModelExpression($"{p.Container.Metadata.Name }.{ p.Metadata.Name}", explorer);

                    var value = Utility.GetFormattedHtml(p, ViewContext, htmlHelper, RenderValueHtml);

                    // check for complex object and set value
                    value = Utility.GetComplexValue(p, value, ViewContext, htmlHelper, RenderValueHtml);

                    // render the cell
                    var cell = new TagBuilder("td");
                    //TODO: Uncomment in order to hide this on mobile when this works (FormIndex)
                    //cell.Attributes.Add("class", styles.TableCellHideMobile);
                    cell.InnerHtml.AppendHtml(value);
                    row.InnerHtml.AppendHtml(cell);
                }

                // render the buttons cell
                if (keyValue != null)
                {
                    var buttons = new TagBuilder("td");
                    var routes = new Dictionary<string, string>() { { keyProperty.ToLower(), keyValue } };
                    if (!string.IsNullOrEmpty(EditPage))
                        buttons.InnerHtml.AppendHtml(GenerateEditButton(explorer, keyProperty, keyValue));
                    if (!string.IsNullOrEmpty(ViewPage))
                        buttons.InnerHtml.AppendHtml(GenerateViewButton(explorer, keyProperty, keyValue));
                    if (!string.IsNullOrEmpty(DeletePage))
                        buttons.InnerHtml.AppendHtml(GenerateDeleteButton(explorer, keyProperty, keyValue));
                    row.InnerHtml.AppendHtml(buttons);
                }
                output.Content.AppendHtml(row);
            }
        }

        protected virtual string GetKeyProperty()
        {
            return Utility.GetKeyProperty(For.Metadata.ElementMetadata.Properties);
        }

        protected virtual void CreateHeaders(TagHelperOutput output, List<string> hideProperties)
        {
            CreateHeadersCore(output, hideProperties, For.Metadata.ElementMetadata.Properties);
        }

        protected void CreateHeadersCore(TagHelperOutput output, List<string> hideProperties, Microsoft.AspNetCore.Mvc.ModelBinding.ModelPropertyCollection properties)
        {
            // create headers
            var headerRow = new TagBuilder("tr") { TagRenderMode = TagRenderMode.Normal };
            foreach (var p in properties)
            {

                // test against hide list
                if (hideProperties != null && hideProperties.Contains(p.PropertyName))
                    continue;

                if (!Utility.DisplayForView(p))
                    continue;

                // render the cell
                var headerCell = new TagBuilder("th");
                //TODO: Uncomment in order to hide this on mobile when this works (FormIndex)
                //cell.Attributes.Add("class", styles.TableCellHideMobile);
                if (p.DisplayName != null)
                    headerCell.InnerHtml.AppendHtml(p.DisplayName);
                else
                    headerCell.InnerHtml.Append(p.Name);
                headerRow.InnerHtml.AppendHtml(headerCell);
            }
            // render one last cell for the buttons
            var lastHeader = new TagBuilder("th");
            headerRow.InnerHtml.AppendHtml(lastHeader);
            output.Content.AppendHtml(headerRow);
        }

        protected virtual IHtmlContent GenerateViewButton(ModelExplorer itemExplorer, string keyProperty, string keyValue)
        {
            var routes = new Dictionary<string, string>()
            {
                { keyProperty.ToLower(), keyValue }
            };
            if (CommandsEnabled)
                routes.Add("command", viewCommand);

            return tg.GenerateAnchorTagHelper(ViewPage, ViewPage, styles.ButtonView, routes);
        }

        protected virtual IHtmlContent GenerateEditButton(ModelExplorer itemExplorer, string keyProperty, string keyValue)
        {
            var routes = new Dictionary<string, string>()
            {
                { keyProperty.ToLower(), keyValue }
            };

            if (CommandsEnabled)
                routes.Add("command", editCommand);

            return tg.GenerateAnchorTagHelper(EditPage, EditPage, styles.ButtonEdit, routes);
        }

        protected virtual IHtmlContent GenerateDeleteButton(ModelExplorer itemExplorer, string keyProperty, string keyValue)
        {
            var routes = new Dictionary<string, string>()
            {
                { keyProperty.ToLower(), keyValue }
            };

            if (CommandsEnabled)
                routes.Add("command", deleteCommand);

            return tg.GenerateAnchorTagHelper(DeletePage, DeletePage, styles.ButtonDelete, routes);
        }
    }
}
