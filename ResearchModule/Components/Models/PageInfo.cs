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
        /// кол-во объектов на странице
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

        public PageInfo()
        {
            PageSize = 10;
        }

        /// <summary>
        /// Пагинация
        /// </summary>
        /// <param name="number">Номер текущей страницы</param>
        /// <param name="items">Всего объектов</param>
        public PageInfo(int number, int items) : base()
        {
            PageNumber = number;
            TotalItems = items;
            
        }
    }
}
