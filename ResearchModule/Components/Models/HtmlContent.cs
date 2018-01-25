using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models
{
    /// <summary>
    /// Преобразование в IHtmlContent
    /// </summary>
    public static class HtmlContent
    {
        private static IHtmlContent Render<T>(Func<T, IHtmlContent> helper, T item = default(T)) {
            return helper(item);
        }

        /// <summary>
        /// Преобразование в IHtmlContent
        /// </summary>
        /// <param name="htmlContent">Пример использования 
        /// HtmlContent.ToHtmlContent(@<text><h4>текст</h4></text>)</param>
        /// <returns></returns>
        public static IHtmlContent ToHtmlContent(Func<object, IHtmlContent> htmlContent)
        {
            return Render(htmlContent);
        }
    }
    
}
