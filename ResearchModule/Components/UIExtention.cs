using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ResearchModule.Components
{
    public static class UIExtention
    {
        private const string Components  = "Components/";
       

        #region Button

        /// <summary>
        /// Кнопка
        /// </summary>
        /// <param name="routeValues">new Dictionary<string, object> { { "enctype", "multipart/form-data" }, { "data-ajax", "false"} })</param>
        /// <returns></returns>
        public static IHtmlContent Button(this IHtmlHelper html, string id, string value, IHtmlContent icon = null, object routeValues = null)
        {
            return Button(html, id, value, icon, new RouteValueDictionary(routeValues));
        }

        /// <summary>
        /// Кнопка
        /// </summary>
        /// <param name="routeValues">new { onClick ="onClick(this)" }</param>
        /// <returns></returns>
        public static IHtmlContent Button(this IHtmlHelper html, string id, string value, IHtmlContent icon = null, RouteValueDictionary routeValues = null)
        {
            var tagBuilder = new TagBuilder("button");
            tagBuilder.GenerateId(id, id);
            tagBuilder.AddCssClass("btn-default");
            tagBuilder.AddCssClass("btn");
            tagBuilder.InnerHtml.AppendHtml(icon);
            tagBuilder.InnerHtml.AppendHtml(value);

            if (routeValues != null)
            {
                tagBuilder.MergeAttributes(routeValues, true);
            }
            tagBuilder.RenderSelfClosingTag();

            return tagBuilder;
        }


        #endregion

        #region Icon

        /// <summary>
        /// Иконка
        /// </summary>
        /// <param name="iconName">Название иконки</param>
        /// <returns></returns>
        public static IHtmlContent Icon(this IHtmlHelper html, string iconName)
        {
            var tagBuilder = new TagBuilder("span");
            var className = string.Format("glyphicon glyphicon-{0}", iconName);
            tagBuilder.AddCssClass(className);
            tagBuilder.RenderSelfClosingTag();

            return tagBuilder;
        }

        /// <summary>
        /// Иконка
        /// </summary>
        /// <param name="iconName">Название иконки</param>
        /// <param name="routeValues">new { onClick ="onClick(this)" }</param>
        /// <returns></returns>
        public static IHtmlContent Icon(this IHtmlHelper html, string iconName, object routeValues)
        {
            var tagBuilder = new TagBuilder("span");
            var className = string.Format("glyphicon glyphicon-{0}", iconName);
            tagBuilder.AddCssClass(className);
            if (routeValues != null)
            {
                tagBuilder.MergeAttributes(new RouteValueDictionary(routeValues), true);
            }
            tagBuilder.RenderSelfClosingTag();

            return tagBuilder;
        }


        #endregion

        #region SelectList

        public static IHtmlContent Option(this IHtmlHelper html, object value, bool selected, string text)
        {
            var tagBuilder = new TagBuilder("option");
            tagBuilder.InnerHtml.AppendHtml(text);
            var dictionary = new Dictionary<string, object>() { { "value", value } };
            if (selected)
            {
                dictionary.Add("selected", "");
            }
            tagBuilder.MergeAttributes(dictionary, true);
            tagBuilder.RenderSelfClosingTag();

            return tagBuilder;
        }

        public static IHtmlContent SelectList(this IHtmlHelper html, ResearchModule.Models.SelectList selectList, bool multiple = false)
        {
            var tagBuilder = new TagBuilder("select");
            tagBuilder.AddCssClass("form-control selectpicker selectpicker_" + selectList.GetName());
            var countElem = selectList.Elements.Count == 0;
            foreach (var elem in selectList.Elements)
            {
                tagBuilder.InnerHtml.AppendHtml(html.Option(elem.Value, elem.Selected, elem.Text));
            }
            var dictionary = new RouteValueDictionary(new { title = "Ничего не выбрано", name = selectList.GetName()});
            if (multiple)
            {
                dictionary.Add("multiple", "");
            }
            tagBuilder.MergeAttributes(dictionary, true);
            tagBuilder.RenderSelfClosingTag();

            return tagBuilder;
        }

        #endregion


        public static IHtmlContent File(this IHtmlHelper html, string name, string displayName = null, string description = null)
        {
            html.ViewData["filename"] = name;
            html.ViewData["description"] = description;
            html.ViewData["displayname"] = displayName;
            return html.Partial(Components+"File");
        }

        public static IHtmlContent FileFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            Func<ModelMetadata, string> propertyName = m => m.PropertyName;
            var modelExplorer = ExpressionMetadataProvider.FromLambdaExpression(expression, html.ViewData, html.MetadataProvider);
            if (modelExplorer != null && modelExplorer.Metadata != null)
            {
                html.ViewData["filename"] = propertyName(modelExplorer.Metadata);
                var name = modelExplorer.GetExplorerForProperty("FileName");
                html.ViewData["displayname"] = name == null ? "" : name.Model;
            }
            return html.Partial(Components + "File");

        }

        public static IHtmlContent FileFor<TModel, TValue, TDisplay>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Expression<Func<TModel, TDisplay>> displayName)
        {
            Func<ModelMetadata, string> propertyName = m => m.PropertyName;
            var explorerFile = GetExplorer(html, expression);
            if (explorerFile != null && explorerFile.Metadata != null)
            {
                html.ViewData["filename"] = propertyName(explorerFile.Metadata);
                var name = explorerFile.GetExplorerForProperty("FileName");
                object fileDisplayName = null;
                if (name != null && name.Model != null)
                {
                    fileDisplayName = name.Model;
                }
                else
                {
                    var explorerDisplay = GetExplorer(html, displayName);
                    if (explorerDisplay != null && explorerDisplay.Model != null)
                    {
                        fileDisplayName = explorerDisplay.Model;
                    }
                }
                html.ViewData["displayname"] = fileDisplayName;
            }
            return html.Partial(Components + "File");

        }


        private static ModelExplorer GetExplorer<TModel, TValue>(IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return ExpressionMetadataProvider.FromLambdaExpression(expression, html.ViewData, html.MetadataProvider);

        }

        private static IHtmlContent MetaDataFor<TModel, TValue>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            Func<ModelMetadata, string> property)
        {
            if (html == null) throw new ArgumentNullException(nameof(html));
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var modelExplorer = ExpressionMetadataProvider.FromLambdaExpression(expression, html.ViewData, html.MetadataProvider);
            if (modelExplorer == null) throw new InvalidOperationException($"Failed to get model explorer for {ExpressionHelper.GetExpressionText(expression)}");
            return new HtmlString(property(modelExplorer.Metadata));
        }


        public static IHtmlContent DisplayNameFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.MetaDataFor(expression, m => m.DisplayName);
        }

        public static string NameFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            Func<ModelMetadata, string> propertyName = m => m.PropertyName;
            var modelExplorer = ExpressionMetadataProvider.FromLambdaExpression(expression, html.ViewData, html.MetadataProvider);
            return modelExplorer != null ? propertyName(modelExplorer.Metadata) : "";
        }

    }
    
}
