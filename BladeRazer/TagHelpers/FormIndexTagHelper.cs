using BladeRazor.Attributes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

            // can we conextualise the htmlhelper - if not do not render html values
            // TODO: it may be worthwhile setting a seperate bool here so we do not conflate these two issues
            if (htmlHelper is IViewContextAware ht)
                ht.Contextualize(ViewContext);
            else
                RenderValueHtml = false;

            // setup properties to hide
            var hideProperties = HideProperties?.Split(',').Select(p => p.Trim()).ToList();

            // create headers
            string keyProperty = null;
            var headerRow = new TagBuilder("tr") { TagRenderMode = TagRenderMode.Normal };
            foreach (var p in For.Metadata.ElementMetadata.Properties)
            {
                // test for key
                var keyAttribute = Utility.GetAttribute<KeyAttribute>(p);
                if (keyAttribute != null)
                    keyProperty = p.Name;

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

            // if we do not have a key search for a property called Id
            if (string.IsNullOrWhiteSpace(keyProperty))
                keyProperty = For.Metadata.ElementMetadata.Properties.Where(p => p.Name.ToLower() == "id").FirstOrDefault()?.Name;

            // render one last cell for the buttons
            var lastHeader = new TagBuilder("th");
            headerRow.InnerHtml.AppendHtml(lastHeader);
            output.Content.AppendHtml(headerRow);

            // now loop through items      
            var collection = (ICollection)For.Model;
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
