using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models.Card
{
    public interface IRow
    {
        string id { get; set; }
        string className { get; set; }
        string size { get; set; }
        HtmlContentBuilder content { get; set; }

        #region Установка свойств

        /// <summary>
        /// Установка длины строки
        /// </summary>
        /// <param name="size">шаблон "col-md-{0}"</param>
        /// <returns></returns>
        IRow Size(long size);

        /// <summary>
        /// Установка длины
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        IRow Size(string size);

        /// <summary>
        /// Установка класса
        /// </summary>
        /// <param name="className">название класса</param>
        /// <returns></returns>
        IRow Class(string className);

        /// <summary>
        /// Установка Id строки
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IRow Id(string id);

        #endregion

        /// <summary>
        /// Добавление строки
        /// </summary>
        /// <returns></returns>
        IRow For();
    }
}
