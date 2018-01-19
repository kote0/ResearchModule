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

namespace ResearchModule.Components.Models
{
    public class Card
    {
        private IHtmlHelper html;
        public string id { get; set; }
        public string headerName { get; set; }
        public string style { get; set; }
        public List<CardRow> rows { get; set; }

        public Card(IHtmlHelper html, string id)
        {
            this.html = html;
            this.rows = new List<CardRow>();
            this.id = id;
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

        public Card Row(Action<Card> bild)
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

        //[Obsolete("Лучше не использовать из-за FindLastIndex")]
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
    }

    public class CardRow
    {
        public string className { get; set; }
        public string id;
        public bool isRow = true;
        public HtmlContentBuilder contentText { get; set; }
        public List<CardRow> contents { get; set; }

        public string ContentSize = "col-md-12";

        public CardRow() {
            contents = new List<CardRow>();
            contentText = new HtmlContentBuilder();
        }

        public async Task<CardRow> RowAsync(Task<IHtmlContent> content, long size)
        {
            var cardRow = new CardRow();
            cardRow.Size(size);
            cardRow.Content(await content);
            this.contents.Add(cardRow);
            return cardRow;
        }

        public CardRow Row(string content)
        {
            var htmlContent = new HtmlContentBuilder().AppendHtml(content);
            return Row(htmlContent);
        }

        public CardRow Row(IHtmlContent content)
        {
            var cardRow = new CardRow().Content(content);
            this.contents.Add(cardRow);
            return cardRow;
        }

        public CardRow Content(string content)
        {
            var htmlContent = new HtmlContentBuilder().AppendHtml(content);
            Content(htmlContent);
            return this;
        }

        public CardRow Content(IHtmlContent content)
        {
            var htmlContent = new HtmlContentBuilder().AppendHtml(content);
            Content(htmlContent);
            return this;
        }

        public CardRow Content(IHtmlContentBuilder content)
        {
            contentText.AppendHtml(content);
            return this;
        }

        public CardRow Size(long size)
        {
            this.ContentSize = string.Format("col-md-{0}", size);
            return this;
        }

        public CardRow Size(string size)
        {
            this.ContentSize = size;
            return this;
        }

        public CardRow Class(string className)
        {
            this.className = className;
            return this;
        }

        public CardRow IsRow(bool isRow)
        {
            this.isRow = isRow;
            return this;
        }
        public CardRow Id(string id)
        {
            this.id = id;
            return this;
        }
    }

}
