using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models.Card
{
    public interface ICardRow 
    {
        string className { get; set; }
        string id { get; set; }
        string ContentSize { get; set; }

        /// <summary>
        /// Нужно ли добавлять класс "row"
        /// </summary>
        bool isRow { get; set; }
        HtmlContentBuilder contentText { get; set; }
        List<CardRow> contents { get; set; }

        CardRow Row(string content = null);
        CardRow Row(IHtmlContent content);
        CardRow Content(string content);
        CardRow Content(IHtmlContent content);
        CardRow Content(IHtmlContentBuilder content);

        #region Установка свойств
        /// <summary>
        /// Длина строки
        /// </summary>
        /// <param name="size">string.Format("col-md-{0}", size)</param>
        /// <returns></returns>
        CardRow Size(long size);
        /// <summary>
        /// Длина строки
        /// </summary>
        /// <param name="size">По умолчанию = "col-md-12"</param>
        /// <returns></returns>
        CardRow Size(string size);
        CardRow IsRow(bool isRow);
        CardRow Id(string id);
        #endregion
    }
}
