using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models.Card
{
    public class CardRow : ICardRow
    {
        public string className { get; set; }
        public string id { get; set; }
        public bool isRow { get; set; }
        public HtmlContentBuilder contentText { get; set; }
        public List<CardRow> contents { get; set; }
        public string ContentSize { get; set; }

        public CardRow()
        {
            contents = new List<CardRow>();
            contentText = new HtmlContentBuilder();
            ContentSize = "col-md-12";
            isRow = true;
        }

        #region Работа с строками и контентом 

        
        public CardRow Row(string content = null)
        {
            if (string.IsNullOrEmpty(content))
            {
                content = "";
            }
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
            contentText.AppendHtml(content);
            return this;
        }

        public CardRow Content(IHtmlContent content)
        {
            contentText.AppendHtml(content);
            return this;
        }

        public CardRow Content(IHtmlContentBuilder content)
        {
            contentText.AppendHtml(content);
            return this;
        }

        #endregion

        #region Установка свойств
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
        #endregion

    }
}
