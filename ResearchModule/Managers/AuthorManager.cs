using Microsoft.AspNetCore.Html;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class AuthorManager : BaseManager
    {
        public void Create(List<Author> authors)
        {
            foreach (var author in authors)
            {
                if (author.IsValid())
                    Create(author);
            }
        }
        public StringBuilder DropbdownList(List<Author> authors)
        {
            if (authors == null) return null;
            StringBuilder text = new StringBuilder();
            text.Append("<ul class='dropdown-menu'>");
            foreach(var author in authors)
            {
                text.AppendFormat("<li class='dropdown-group-item'>{0} {1} {2}</li>", author.Surname, author.Name, author.LastName);
            }
            text.Append("</ul>");
            return text;
        }
        public bool IsValid(List<Author> authors)
        {
            if (authors.Count == 0) return false;
            foreach(var author in authors)
            {
                if (!author.IsValid())
                    return false;
            }
            return true;
        }
    }
}
