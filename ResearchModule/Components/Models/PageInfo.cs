using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models
{
    /// <summary>
    /// Пагинация
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// Номер текущей страницы
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Кол-во объектов на странице
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Всего объектов
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Всего страниц
        /// </summary>
        public int TotalPages 
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }

        /// <summary>
        /// Количество элементов на странице
        /// </summary>
        public PageInfo()
        {
            PageSize = 10;
        }

        /// <summary>
        /// Ссылка
        /// </summary>
        private string url { get; set; }

        /// <summary>
        /// Id Формы
        /// </summary>
        public string DataId { get; set; }

        /// <summary>
        /// Пагинация
        /// </summary>
        /// <param name="number">Номер текущей страницы</param>
        /// <param name="items">Всего объектов</param>
        public PageInfo(int number, int items) : base()
        {
            PageNumber = number;
            TotalItems = items;
            PageSize = 10;

        }

        /// <summary>
        /// Установить ссылку
        /// </summary>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        public void SetUrl(string action, string controller)
        {
            url = string.Format("/{1}/{0}?first=", action, controller);
        }

        /// <summary>
        /// Получить ссылку
        /// </summary>
        /// <param name="first">Номер страницы</param>
        /// <returns></returns>
        public string GetUrl(/*int first*/)
        {
            return url;//string.Concat(url, first.ToString());
        }

    }
}
