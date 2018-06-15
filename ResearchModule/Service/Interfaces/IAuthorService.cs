using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Service.Interfaces
{
    public interface IAuthorService
    {
        IEnumerable<Author> GetAuthors(int id);
    }
}
