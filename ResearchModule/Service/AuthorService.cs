using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Service
{
    public class AuthorService
    {
        /// <summary>
        /// Сокращенный формат
        /// </summary>
        /// <param name="author"></param>
        /// <returns>Фамилия И.О.</returns>
        public string ToStringFormat(Author author)
        {
            return string.Format("{0} {1}.{2}.", 
                author.Surname, author.Name.Substring(0, 1), 
                author.Lastname.Substring(0, 1));
        }
    }
}

