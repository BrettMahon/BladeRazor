﻿using BladeRazer.Attributes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
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

        public FormEditTagHelper(IHtmlGenerator generator) : base(generator) { }

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

                // TODO: replace this with new templated method
                // get our attributes - test first that this is the default model metadata                
                FormAttribute formAttribute = null;
                if (explorer.Metadata is Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.DefaultModelMetadata meta)
                {
                    var modelAttributes = meta.Attributes;
                    formAttribute = (FormAttribute)modelAttributes.PropertyAttributes.Where(p => p.GetType() == typeof(FormAttribute)).FirstOrDefault();
                }

                var f = new ModelExpression($"{explorer.Container.Metadata.Name }.{ explorer.Metadata.Name}", explorer);

                // if we have a hidden field, generate it and continue
                if (formAttribute?.Type == FormInputType.Hidden)
                {
                    output.Content.AppendHtml(GenerateHiddenTagHelper(f));
                    continue;
                }

                TagBuilder group = new TagBuilder("div");
                group.Attributes.Add("class", "form-group");
                group.InnerHtml.AppendHtml(GenerateLabel(f));
                group.InnerHtml.AppendHtml(GenerateContent(f, formAttribute));
                group.InnerHtml.AppendHtml(GenerateValidation(f));
                output.Content.AppendHtml(group);
            }
        }

        protected IHtmlContent GenerateContent(ModelExpression f, FormAttribute formAttribute)
        {
            return formAttribute?.Type switch
            {
                FormInputType.TextArea => GenerateTextAreaTagHelper(f, formAttribute.TextAreaRows),
                FormInputType.Hidden => GenerateHiddenTagHelper(f),
                FormInputType.Select => GenerateSelectContent(f, formAttribute),
                _ => GenerateDefaultContent(f, formAttribute)
            };
        }

        protected IHtmlContent GenerateSelectContent(ModelExpression f, FormAttribute formAttribute)
        {
            // do we have a key in the attribute
            if (ItemsDictionary != null && ItemsDictionary.ContainsKey(formAttribute.SelectItemsKey))
            {
                // does the key match our dictionary
                if (ItemsDictionary.ContainsKey(formAttribute.SelectItemsKey))
                    return GenerateSelectTagHelper(f, ItemsDictionary[formAttribute.SelectItemsKey], formAttribute.SelectOptionName, formAttribute.SelectOptionValue);
            }

            // if not simply use the supplied items
            return GenerateSelectTagHelper(f, this.Items, formAttribute.SelectOptionName, formAttribute.SelectOptionValue);
        }

        protected IHtmlContent GenerateDefaultContent(ModelExpression f, FormAttribute formAttribute)
        {
            //TODO: Check for datatype multiline and render a textarea if true
            if (f.Model != null && f.Model.GetType() == typeof(bool))
            {
                var content = GenerateCheckboxGroup(f);
                return content;
            }
            else
                return GenerateInputTagHelper(f);
        }
    }



}