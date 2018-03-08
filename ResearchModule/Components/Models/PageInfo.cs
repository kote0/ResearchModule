﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models
{
    public class PageInfo
    {
        /// <summary>
        /// номер текущей страницы
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// кол-во объектов на странице
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// всего объектов
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// // всего страниц
        /// </summary>
        public int TotalPages 
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }

        public PageInfo()
        {
            PageSize = 10;
        }
    }
}
