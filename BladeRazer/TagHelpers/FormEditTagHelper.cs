using BladeRazer.Attributes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BladeRazer.TagHelpers
{
    [HtmlTargetElement("form-edit", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormEditTagHelper : FormBaseTagHelper
    {
        [HtmlAttributeName("asp-items-dictionary")]
        public Dictionary<string, IEnumerable<SelectListItem>> ItemsDictionary { get; set; }

        [HtmlAttributeName("asp-items")]
        public IEnumerable<SelectListItem> Items { get; set; } = new List<SelectListItem>();

        public FormEditTagHelper(IHtmlGenerator generator, IStyles styles = null) : base(generator, styles) { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            // loop through properties
            foreach (var explorer in For.ModelExplorer.Properties)
            {
                //  we are not recursing here
                if (explorer.Metadata.IsComplexType)
                    continue;

                // read-only fields are skipped
                if (explorer.Metadata.IsReadOnly)
                    continue;

                // create a model expression from the explorer
                var f = new ModelExpression($"{explorer.Container.Metadata.Name }.{ explorer.Metadata.Name}", explorer);

                // if we have a hidden field, generate it and continue
                var fa = Utility.GetAttribute<FormAttribute>(explorer.Metadata);
                if (fa?.Type == FormInputType.Hidden)
                {
                    output.Content.AppendHtml(tg.GenerateHiddenTagHelper(f));
                    continue;
                }

                // render
                TagBuilder group = new TagBuilder("div");
                group.Attributes.Add("class", styles.FormGroup);
                group.InnerHtml.AppendHtml(tg.GenerateLabel(f));
                group.InnerHtml.AppendHtml(GenerateContent(f, fa));
                group.InnerHtml.AppendHtml(tg.GenerateValidation(f));
                output.Content.AppendHtml(group);
            }
        }

        protected IHtmlContent GenerateContent(ModelExpression f, FormAttribute fa) =>
            fa?.Type switch
            {
                FormInputType.TextArea => tg.GenerateTextAreaTagHelper(f, fa.TextAreaRows),
                FormInputType.Hidden => tg.GenerateHiddenTagHelper(f),
                FormInputType.Select => GenerateSelectContent(f, fa),
                _ => GenerateDefaultContent(f)
            };


        protected IHtmlContent GenerateSelectContent(ModelExpression f, FormAttribute formAttribute)
        {
            // do we have a key in the attribute
            if (ItemsDictionary != null && ItemsDictionary.ContainsKey(formAttribute.SelectItemsKey))
            {
                // does the key match our dictionary
                if (ItemsDictionary.ContainsKey(formAttribute.SelectItemsKey))
                    return tg.GenerateSelectTagHelper(f, ItemsDictionary[formAttribute.SelectItemsKey], formAttribute.SelectOptionName, formAttribute.SelectOptionValue);
            }

            // if not simply use the supplied items
            return tg.GenerateSelectTagHelper(f, this.Items, formAttribute.SelectOptionName, formAttribute.SelectOptionValue);
        }

        protected IHtmlContent GenerateDefaultContent(ModelExpression f)
        {
            // check for multi-line
            var dta = Utility.GetAttribute<DataTypeAttribute>(f.Metadata);
            if (dta != null && dta.DataType == DataType.MultilineText)
                return tg.GenerateTextAreaTagHelper(f, 2);
            // check for boolean
            if (f.Model != null && f.Model.GetType() == typeof(bool))
                return tg.GenerateCheckboxGroup(f);
            // else run default
            return tg.GenerateInputTagHelper(f);
        }
    }
}
