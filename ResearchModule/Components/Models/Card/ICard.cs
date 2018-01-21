using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models.Card
{
    public interface ICard
    {
        string id { get; set; }
        string style { get; set; }
        List<CardRow> rows { get; set; }
        /// <summary>
        /// Заголовок
        /// </summary>
        string headerName { get; set; }
        
        /// <summary>
        /// Раскрытие
        /// </summary>
        bool collapse { get; set; }

        
        //Card Row(Action<Card> bild);
        CardRow For();
        Card For(Action<CardRow> bild);
        IHtmlContent Render();

        #region Установка свойств
        Card Collapse();
        Card Header(string header);
        Card Style(string style);
        #endregion
    }
}