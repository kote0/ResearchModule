using ResearchModule.Models;
using ResearchModule.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class AuthorManager
    {
        private readonly PAuthorRepository paRepository;

        public AuthorManager(PAuthorRepository paRepository)
        {
            this.paRepository = paRepository;
        }

        public IEnumerable<Author> GetAuthors(int id)
        {
            return paRepository.Find(id);
        }
    }
}
