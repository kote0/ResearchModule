using Microsoft.AspNetCore.Html;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    //TODO: Удалить AuthorManager
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

    }
}
