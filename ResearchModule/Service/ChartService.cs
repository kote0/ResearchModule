﻿using ResearchModule.Components.Models;
using ResearchModule.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Service
{
    public class ChartService
    {
        private string[] months { get { return new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" }; } }

        private readonly PublicationManager _publicationManager;

        public ChartService(PublicationManager publicationManager)
        {
            _publicationManager = publicationManager;
        }

        public Chart PublicationCount(DateTime date)
        {
            var data = new List<string>();
            for (var i = 1; i <= 12; i++)
            {
                var count = _publicationManager.Count(p => p.CreateDate.Year == date.Year && p.CreateDate.Month == i);
                data.Add(count.ToString());
            }

            return new Chart()
            {
                Data = ListToFormat(data),
                Label = ListToFormat(months),
                Type = Format("line"),
                DisplayName = Format("Количество публикаций"),
                Id = "PublicationCount"
            };
        }

        private string Format(string str)
        {
            return string.Format("'{0}'", str);
        }

        private string ListToFormat(ICollection<string> list)
        {
            return string.Join(",", list.Select(a => Format(a)));
        }

    }
}
