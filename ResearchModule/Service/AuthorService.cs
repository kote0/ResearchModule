using ResearchModule.Extensions;
using ResearchModule.Models;
using ResearchModule.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchModule.Service
{
    public class AuthorService
    {
        private readonly PARepository paRepository;

        public AuthorService(PARepository paRepository)
        {
            this.paRepository = paRepository;
        }

        public IEnumerable<Author> GetAuthors(int id)
        {
            return paRepository.Find(id);
        }
    }
}

