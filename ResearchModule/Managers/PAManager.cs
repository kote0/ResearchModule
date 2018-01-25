﻿using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    /// <summary>
    /// Таблица, хранящая в себе авторов и их публикации
    /// Связь М:М
    /// </summary>
    public class PAManager : BaseManager
    {
        public void Create(IEnumerable<Author> authors, long pid)
        {
            foreach (var author in authors)
            {
                PA pa = new PA();
                pa.AId = author.Id;
                pa.Weight = author.Weight;
                pa.PId = pid;
                Create(pa);
            }
        }
        public void Create(long aid, long pid)
        {
            PA pa = new PA(pid, aid);
            Create(pa);
        }
        public List<Author> FindAuthorsByPublication(long idPublication)
        {
            var pas = GetByFunction<PA>(pa => pa.PId == idPublication).ToList();
            List<Author> authors = new List<Author>();
            foreach(var item in pas)
            {
                var author = GetByFunction<Author>(a => a.Id == item.AId).FirstOrDefault();
                if (author != null)
                    authors.Add(author);
            }
            return authors;
        }
    }
}
