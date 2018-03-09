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
        private readonly BaseManager manager;

        public PAManager(BaseManager manager)
        {
            this.manager = manager;
        }

        public void Create(IEnumerable<Author> authors, int pid)
        {
            foreach (var author in authors)
            {
                PA pa = new PA();
                pa.AuthorId = author.Id;
                pa.Weight = author.Weight;
                pa.PublicationId = pid;
                manager.Create(pa);
            }
        }
        public void Create(int aid, int pid)
        {
            PA pa = new PA(pid, aid);
            manager.Create(pa);
        }
        public async Task<IEnumerable<Author>> FindAuthorsByPublication(int idPublication)
        {
            var pas = await manager.Get<PA>(pa => pa.PublicationId == idPublication);
            List<Author> authors = new List<Author>();
            foreach (var item in pas)
            {
                var list = await manager.Get<Author>(a => a.Id == item.AuthorId);
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
