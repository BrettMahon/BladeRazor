using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeRazer.TagHelpers
{
    public class TagGenerator
    {
        protected IHtmlGenerator generator;
        protected ViewContext viewContext;
        protected Styles styles;

        public TagGenerator(IHtmlGenerator generator, ViewContext viewContext)
        {
            this.generator = generator;
            this.viewContext = viewContext;
            this.styles = new Styles();
        }

        public TagGenerator(IHtmlGenerator generator, ViewContext viewContext, Styles styles)
        {
            this.generator = generator;
            this.viewContext = viewContext;
            this.styles = styles;
        }

        public virtual TagBuilder GenerateLabel(ModelExpression f) =>
            generator.GenerateLabel(viewContext, f.ModelExplorer, f.Name, null, new { @class = styles.Label });

        public virtual TagBuilder GenerateCheckboxLabel(ModelExpression f) =>
            generator.GenerateLabel(viewContext, f.ModelExplorer, f.Name, null, new { @class = styles.CheckLabel });

        public TagBuilder GenerateValidation(ModelExpression f) =>
            generator.GenerateValidationMessage(viewContext, f.ModelExplorer, f.Name, null, null, new { @class = styles.Validation });

        public TagHelperOutput GenerateInputTagHelper(ModelExpression f)
        {
            var tagHelper = new InputTagHelper(generator)
            {
                For = f,
                Name = f.Name,
                ViewContext = viewContext
            };

            return GenerateTagHelperCore(f, tagHelper, "input", TagMode.SelfClosing);
        }

        public TagHelperOutput GenerateHiddenTagHelper(ModelExpression f)
        {
            var tagHelper = new InputTagHelper(generator)
            {
                For = f,
                Name = f.Name,
                ViewContext = viewContext,
                InputTypeName = "hidden"
            };

            return GenerateTagHelperCore(f, tagHelper, "input", TagMode.SelfClosing, "hidden", "form-control");
        }

        public TagHelperOutput GenerateCheckboxTagHelper(ModelExpression f)
        {
            var tagHelper = new InputTagHelper(generator)
            {
                For = f,
                Name = f.Name,
                ViewContext = viewContext,
                Value = f.Model.ToString().ToLower()
            };

            var output = GenerateTagHelperCore(f, tagHelper, "input", TagMode.SelfClosing, "text", "form-check-input");
            return output;
        }

        public TagHelperOutput GenerateTextAreaTagHelper(ModelExpression f, int rows)
        {
            var tagHelper = new TextAreaTagHelper(generator)
            {
                For = f,
                Name = f.Name,
                ViewContext = viewContext
            };

            var tagOutput = GenerateTagHelperCore(f, tagHelper, "textarea", TagMode.StartTagAndEndTag);
            tagOutput.Attributes.Add(new TagHelperAttribute("rows", rows));
            return tagOutput;
        }

        public TagHelperOutput GenerateAnchorTagHelper(string page, string text, string cssClass, IDictionary<string, string> routeValues)
        {
            var tagHelper = new AnchorTagHelper(generator)
            {
                ViewContext = viewContext,
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

        public TagHelperOutput GenerateSelectTagHelper(ModelExpression f, IEnumerable<SelectListItem> items, string optionName, string optionValue)
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
                ViewContext = viewContext
            };

            var tagOutput = GenerateTagHelperCore(f, tagHelper, "select", TagMode.StartTagAndEndTag);
            return tagOutput;
        }

        public IHtmlContent GenerateCheckboxGroup(ModelExpression f)
        {
            TagBuilder group = new TagBuilder("div");
            group.Attributes.Add("class", "form-check pb-1 pt-2");
            group.InnerHtml.AppendHtml(GenerateCheckboxTagHelper(f));
            group.InnerHtml.AppendHtml(GenerateCheckboxLabel(f, null));
            group.InnerHtml.AppendHtml(GenerateValidation(f, null));
            return group;
        }

        public TagHelperOutput GenerateValidationSummaryTagHelper()
        {
            throw new NotImplementedException();
        }

        protected TagHelperOutput GenerateTagHelperCore(ModelExpression f, TagHelper tagHelper, string tagName, TagMode tagMode) =>
            GenerateTagHelperCore(f, tagHelper, tagName, tagMode, "text", "form-control");

        protected TagHelperOutput GenerateTagHelperCore(ModelExpression f, TagHelper tagHelper, string tagName, TagMode tagMode, string type, string css)
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
            tagOutput.Attributes.Add(new TagHelperAttribute("class", css));
            return tagOutput;
        }
    }
}
