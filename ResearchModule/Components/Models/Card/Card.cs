using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models.Card
{
    public class Card : ICard
    {
        private IHtmlHelper html;
        public string id { get; set; }
        public string headerName { get; set; }
        public string style { get; set; }
        public bool collapse { get; set; }
        public List<CardRow> rows { get; set; }

        public Card(IHtmlHelper html, string id)
        {
            this.html = html;
            this.rows = new List<CardRow>();
            this.id = id;
            this.collapse = false;
        }

        public delegate Card RowCo(Card verb);

        public Card Row(RowCo bild)
        {
            bild(this);
            return this;
        }

        public CardRow For()
        {
            var cardRow = new CardRow();
            rows.Add(cardRow);
            return cardRow;
        }

        [Obsolete("Лучше не использовать из-за FindLastIndex")]
        public Card For(Action<CardRow> bild)
        {
            var row = new CardRow();
            rows.Add(row);
            var index = rows.FindLastIndex(o=>o== row);
            bild(rows[index]);
            return this;
        }

        public IHtmlContent Render()
        {
            return html.Partial("Components/Card", this);
        }

        #region Установка свойств
        public Card Collapse()
        {
            this.collapse = !this.collapse;
            return this;
        }

        public Card Header(string header)
        {
            this.headerName = header;
            return this;
        }

        public Card Style(string style)
        {
            this.style = style;
            return this;
        }
        #endregion
    }
    

}
