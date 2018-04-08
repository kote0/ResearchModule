using ResearchModule.Managers.Interfaces;
using ResearchModule.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    /// <summary>
    /// Таблица, хранящая в себе авторов и их публикации
    /// Связь М:М
    /// </summary>
    public class PAManager
    {
        private readonly BaseManager Manager;

        public PAManager(BaseManager manager)
        {
            this.Manager = manager;
        }

        public void Create(IEnumerable<Author> authors, int pid)
        {
            foreach (var author in authors)
            {
                Create(author.Id, pid, author.Weight);
            }
        }
        public void Create(int aid, int pid, double weight)
        {
            PA pa = new PA();
            pa.AuthorId = aid;
            pa.Weight = weight;
            pa.PublicationId = pid;
            Manager.Create(pa);
        }

        public async Task<IEnumerable<Author>> FindAuthors(int idPublication)
        {
            var pas = await Manager.Get<PA>(pa => pa.PublicationId == idPublication);
            List<Author> authors = new List<Author>();
            foreach (var item in pas)
            {
                var list = await Manager.Get<Author>(a => a.Id == item.AuthorId);
                var author = list.FirstOrDefault();
                if (author != null)
                {
                    author.Weight = item.Weight;
                    author.Selected = true;
                    authors.Add(author);
                }
            }


            return authors;
        }
    }

}
