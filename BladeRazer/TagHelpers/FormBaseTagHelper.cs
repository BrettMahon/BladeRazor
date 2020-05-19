﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public abstract class FormBaseTagHelper : TagHelper
    {
        protected IHtmlGenerator generator;

        public FormBaseTagHelper(IHtmlGenerator generator)
        {
            this.generator = generator;
        }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeName("asp-format")]
        public string Format { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected virtual TagBuilder GenerateLabel(ModelExpression f) =>
           generator.GenerateLabel(ViewContext, f.ModelExplorer, f.Name, null, new { @class = "control-label" });

        protected virtual TagBuilder GenerateCheckboxLabel(ModelExpression f) =>
         generator.GenerateLabel(ViewContext, f.ModelExplorer, f.Name, null, new { @class = "form-check-label" });

        protected TagBuilder GenerateValidation(ModelExpression f) =>
           generator.GenerateValidationMessage(ViewContext, f.ModelExplorer, f.Name, null, null, new { @class = "text-danger" });

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "form-group");


            output.Content.AppendHtml(GenerateLabel(For));
            output.Content.AppendHtml(GenerateTagHelper());
            output.Content.AppendHtml(GenerateValidation(For));
        }

        protected virtual TagBuilder GenerateControl()
        {
            throw new NotImplementedException();
        }

        protected virtual TagHelperOutput GenerateTagHelper()
        {
            throw new NotImplementedException();
        }

        protected TagHelperOutput GenerateHiddenTagHelper(ModelExpression f)
        {
            var tagHelper = new InputTagHelper(generator)
            {
                For = f,
                Name = f.Name,
                ViewContext = ViewContext,
                InputTypeName = "hidden"
            };

            return GenerateTagHelperCore(f, tagHelper, "input", TagMode.SelfClosing, "hidden", "form-control");
        }

        protected IHtmlContent GenerateCheckboxGroup(ModelExpression f)
        {
            TagBuilder group = new TagBuilder("div");
            group.Attributes.Add("class", "form-check pb-1 pt-2");
            group.InnerHtml.AppendHtml(GenerateCheckboxTagHelper(f));
            group.InnerHtml.AppendHtml(GenerateCheckboxLabel(f));
            group.InnerHtml.AppendHtml(GenerateValidation(f));
            return group;
        }

        protected TagHelperOutput GenerateCheckboxTagHelper(ModelExpression f)
        {
            var tagHelper = new InputTagHelper(generator)
            {
                For = f,
                Name = f.Name,
                ViewContext = ViewContext,
                Value = f.Model.ToString().ToLower()
            };

            var output = GenerateTagHelperCore(f, tagHelper, "input", TagMode.SelfClosing, "text", "form-check-input");
            return output;
        }

        protected TagHelperOutput GenerateInputTagHelper(ModelExpression f)
        {
            var tagHelper = new InputTagHelper(generator)
            {
                For = f,
                Name = f.Name,
                ViewContext = ViewContext
            };

            return GenerateTagHelperCore(f, tagHelper, "input", TagMode.SelfClosing);
        }


        protected TagHelperOutput GenerateValidationSummaryTagHelper()
        {
            throw new NotImplementedException();
        }        

        protected TagHelperOutput GenerateAnchorTagHelper(string page, string text, string cssClass, IDictionary<string, string> routeValues)
        {
            var tagHelper = new AnchorTagHelper(generator)
            {
                ViewContext = ViewContext,
                Page = page,
                RouteValues = routeValues
            };

            var tagOutput = new TagHelperOutput("a", new TagHelperAttributeList(),
                (useCachedResult, encoder) => Task.Factory.StartNew<TagHelperContent>(() => new DefaultTagHelperContent()));
            tagOutput.Content.AppendHtml(text);

            var anchorContext = new TagHelperContext(new TagHelperAttributeList(new[]
                { new TagHelperAttribute("asp-page", new HtmlString(page)) }),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString());

            tagHelper.Process(anchorContext, tagOutput);
            tagOutput.Attributes.Add(new TagHelperAttribute("class", cssClass));
            return tagOutput;
        }

        protected TagHelperOutput GenerateAnchorTagHelper(string page, string text, string cssClass) =>
            GenerateAnchorTagHelper(page, text, cssClass, null);

        protected TagHelperOutput GenerateTextAreaTagHelper(ModelExpression f, int rows)
        {
            var tagHelper = new TextAreaTagHelper(generator)
            {
                For = f,
                Name = f.Name,
                ViewContext = ViewContext
            };

            var tagOutput = GenerateTagHelperCore(f, tagHelper, "textarea", TagMode.StartTagAndEndTag);
            tagOutput.Attributes.Add(new TagHelperAttribute("rows", rows));
            return tagOutput;
        }

        protected TagHelperOutput GenerateSelectTagHelper(ModelExpression f, IEnumerable<SelectListItem> items, string optionName, string optionValue)
        {
            var itemsList = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(optionName))
                itemsList.Add(new SelectListItem(optionName, optionValue));
            itemsList.AddRange(items);

            // set selected item - based on value
            if (f.Model != null)
            {
                var selected = itemsList.Where(i => i.Value?.ToString() == f.Model.ToString()).FirstOrDefault();
                if (selected != null)
                    selected.Selected = true;
            }
            var tagHelper = new SelectTagHelper(generator)
            {
                For = f,
                Name = f.Name,
                Items = itemsList,
                ViewContext = ViewContext
            };

            var tagOutput = GenerateTagHelperCore(f, tagHelper, "select", TagMode.StartTagAndEndTag);
            return tagOutput;
        }



        protected TagHelperOutput GenerateTagHelperCore(ModelExpression f, TagHelper tagHelper, string tagName, TagMode tagMode)
        {
            return GenerateTagHelperCore(f, tagHelper, tagName, tagMode, "text", "form-control");
        }

        protected TagHelperOutput GenerateTagHelperCore(ModelExpression f, TagHelper tagHelper, string tagName, TagMode tagMode, string type, string cssClass)
        {
            var tagOutput = new TagHelperOutput(
               tagName, new TagHelperAttributeList(),
               (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(result: null))
            { TagMode = tagMode };

            var attributes = new TagHelperAttributeList();
            if (f != null)
            {
                attributes = new TagHelperAttributeList
                {
                    { "name",  f.Name },
                    { "type",  type },
                    { "value", f.Model?.ToString().ToLower() }
                };
            }
            var tagContext = new TagHelperContext(
                attributes,
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString());

            tagHelper.Process(tagContext, tagOutput);
            tagOutput.Attributes.Add(new TagHelperAttribute("class", cssClass));
            return tagOutput;
        }

        protected T GetAttribute<T>(ModelMetadata p) where T : Attribute
        {
            T attribute = null;
            if (p is Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.DefaultModelMetadata meta)
                attribute = (T)meta.Attributes.PropertyAttributes.Where(p => p.GetType() == typeof(T)).FirstOrDefault();
            return attribute;
        }
    }
}