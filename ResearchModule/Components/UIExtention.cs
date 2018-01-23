using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using ResearchModule.Components.Models.Card;
using ResearchModule.Managers;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        #region Card

        public static Card Card(this IHtmlHelper html, string id)
        {
            return new Card(html, id);
        }
        
        #endregion
    }

}
