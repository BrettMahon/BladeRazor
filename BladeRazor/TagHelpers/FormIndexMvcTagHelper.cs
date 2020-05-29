// Copyright (c) Brett Lyle Mahon. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;

namespace BladeRazor.TagHelpers
{
    [HtmlTargetElement("form-index-mvc", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormIndexMvcTagHelper : FormIndexTagHelper
    {
        /// <summary>
        /// This is used when working with a dynamic model and an empty list
        /// </summary>
        [HtmlAttributeName("asp-headers-for")]
        public ModelExpression HeadersFor { get; set; } = null;

        public FormIndexMvcTagHelper(IHtmlGenerator generator, IHtmlHelper htmlHelper, IStyles styles = null) : base(generator, htmlHelper, styles) { }


        protected override string GetKeyProperty()
        {
            // if we do not have the headers defined
            if (HeadersFor == null || HeadersFor.Model == null)
                return base.GetKeyProperty();

            // now we get the metadata from our concrete model - allows for duck typing              
            var meta = HeadersFor.Metadata.GetMetadataForType(HeadersFor.Model.GetType());
            return Utility.GetKeyProperty(meta.Properties);
        }

        protected override void CreateHeaders(TagHelperOutput output, List<string> hideProperties)
        {
            // if we do not have the headers defined
            if (HeadersFor == null || HeadersFor.Model == null)
            {
                base.CreateHeaders(output, hideProperties);
                return;
            }

            // now we get the metadata from our concrete model - allows for duck typing              
            var meta = HeadersFor.Metadata.GetMetadataForType(HeadersFor.Model.GetType());
            //var explorer = HeadersFor.Metadata.GetModelExplorerForType(HeadersFor.Model.GetType(), HeadersFor.Model);
            CreateHeadersCore(output, hideProperties, meta.Properties);
        }

        protected override IHtmlContent GenerateViewButton(ModelExplorer itemExplorer, string keyProperty, string keyValue)
        {
            var routes = new Dictionary<string, string>()
            { { keyProperty.ToLower(), keyValue } };

            return tg.GenerateAnchorActionTagHelper(ViewPage, ViewPage, styles.ButtonView, routes);
        }

        protected override IHtmlContent GenerateEditButton(ModelExplorer itemExplorer, string keyProperty, string keyValue)
        {
            var routes = new Dictionary<string, string>()
            { { keyProperty.ToLower(), keyValue } };


            return tg.GenerateAnchorActionTagHelper(EditPage, EditPage, styles.ButtonEdit, routes);
        }

        protected override IHtmlContent GenerateDeleteButton(ModelExplorer itemExplorer, string keyProperty, string keyValue)
        {
            var routes = new Dictionary<string, string>()
            { { keyProperty.ToLower(), keyValue } };

            return tg.GenerateAnchorActionTagHelper(DeletePage, DeletePage, styles.ButtonDelete, routes);
        }
    }
}
