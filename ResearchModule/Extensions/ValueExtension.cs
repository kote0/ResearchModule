using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchModule.Extensions
{
    public static class ValueExtension
    {
        #region Publication

        public static bool IsValid(this Publication publication)
        {
            return !string.IsNullOrEmpty(publication.PublicationName)
                && publication.PublicationTypeId != 0
                && !string.IsNullOrEmpty(publication.OutputData)
                && publication.PublicationFile != null;

            //&& publication.PublicationPartition != 0
            //&& publication.PublicationForm != 0
        }

        public static bool Contains(this Publication publication, string text)
        {
            if (publication.IsValid() && !string.IsNullOrEmpty(text))
            {
                return publication
                    .PublicationName.ToLower()
                    .Contains(text.ToLower());
            }

            return false;
        }

        #endregion

        #region Author


        public static bool IsValid(this Author author)
        {
            if (string.IsNullOrEmpty(author.Surname)
                || string.IsNullOrEmpty(author.Name)
                || string.IsNullOrEmpty(author.Lastname))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Сокращенный формат Фамилия И.О.
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
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
            if (authors == null) return "";
            var countAuthors = authors.Count();
            StringBuilder str = new StringBuilder();
            for (var i = 0; i < countAuthors; i++)
            {
                str.AppendFormat("{0}{1}", i != 0 && i != countAuthors
                    ? ","
                    : "", authors.ElementAt(i).ToStringFormat());
            }
            return str.ToString();
        }

        public static bool Contains(this Author author, string text)
        {
            if (author.IsValid() && !string.IsNullOrEmpty(text))
            {
                text = text.ToLower();
                return author.Lastname.ToLower().Contains(text)
                    || author.Surname.ToLower().Contains(text)
                    || author.Name.ToLower().Contains(text);
            }

            return false;
        }

        #endregion

    }
}
