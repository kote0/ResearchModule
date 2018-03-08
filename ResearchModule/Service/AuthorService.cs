using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchModule.Service
{
    public static class AuthorService
    {
        /// <summary>
        /// Сокращенный формат
        /// </summary>
        /// <param name="author"></param>
        /// <returns>Фамилия И.О.</returns>
        public static string ToStringFormat(this Author author)
        {
            return string.Format("{0} {1}.{2}.", 
                author.Surname, author.Name.Substring(0, 1), 
                author.Lastname.Substring(0, 1));
        }

        /// <summary>
        /// Перечесление авторов через запятую
        /// </summary>
        /// <param name="authors"></param>
        /// <returns></returns>
        public static string ToStringThroughComma(this IEnumerable<Author> authors)
        {
            var countAuthors = authors.Count();
            StringBuilder str = new StringBuilder();
            for (var i = 0; i < countAuthors; i++)
            {
                str.AppendFormat("{0}{1}", i != 0 && i != countAuthors ? "," : "", authors.ElementAt(i).ToStringFormat());
            }
            return str.ToString();
        }
    }
}

