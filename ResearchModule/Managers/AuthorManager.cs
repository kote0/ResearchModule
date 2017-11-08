using Microsoft.AspNetCore.Html;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class AuthorManager : BaseManager<Author>
    {
        public override List<Author> GetAll()
        {
            return _db.Author.ToList();
        }
        public List<Author> GetAuthors(string character)
        {
            if (character == null) return null;
            var text = character.ToLower();
            var authors = _db.Author.Where(a => a.LastName.ToLower().Contains(text)
                || a.Surname.ToLower().Contains(text)
                || a.Name.ToLower().Contains(text))
                .ToList();
            return authors;
        }
        public StringBuilder ListAuthors(List<Author> authors)
        {
            if (authors.Count == 0) return null;
            StringBuilder text = new StringBuilder();
            text.Append("<ul class='list-group'>");
            foreach(var author in authors)
            {
                text.AppendFormat("<li class='list-group-item'>{0} {1} {2}</li>", author.Surname, author.Name, author.LastName);
            }
            text.Append("</ul>");
            return text;
        }
    }
}
