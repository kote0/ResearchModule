using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Routing;
using ResearchModule.Managers;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

        public static IHtmlContent SelectList(this IHtmlHelper html, ResearchModule.Models.SelectList selectList, string title = null)
        {
            var tagBuilder = new TagBuilder("select");
            tagBuilder.AddCssClass("form-control selectpicker_" + selectList.GetName());

            foreach (var elem in selectList.Elements)
            {
                tagBuilder.InnerHtml.AppendHtml(html.Option(elem.Value, elem.Selected, elem.Text));
            }
            tagBuilder.MergeAttributes(new RouteValueDictionary(new { title = title ?? "Ничего не выбрано", name=selectList.GetName() }), true);
            tagBuilder.RenderSelfClosingTag();

            return tagBuilder;
        }

        #endregion


        public static IHtmlContent File(this IHtmlHelper html, string name, string description = null)
        {
            html.ViewData["filename"] = name;
            html.ViewData["description"] = description;
            return html.Partial(Components+"File");
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

    }
    
}
