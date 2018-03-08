using System;
using System.Collections.Generic;
using System.Linq;
using ResearchModule.Models;
using System.Threading.Tasks;
using ResearchModule.Managers;

namespace ResearchModule.Service
{
    public class SelectListService
    {
        private readonly BaseManager manager;
        private readonly PublicationElements publicationElements;

        public SelectListService(BaseManager manager, PublicationElements publicationElements)
        {
            this.publicationElements = publicationElements;
            this.manager = manager;
        }

        #region select list

        public SelectList LoadSelectAuthor(int id = 0)
        {
            var list = manager.GetByFunction<Author>(a => a.IsValid()) //сделать асинхронным
                .Select(a => newSelectListItem(a.ToStringFormat(),a.Id, id));
            return selectListCreate(list, "Author");
        }


        public SelectList LoadSelectPublicationPartition(int id = 0)
        {
            var list = publicationElements.Partitions.Select(a => newSelectListItem(a.Name, a.Id, id));
            return selectListCreate(list, "Publication.PublicationPartition");
        }
       

        public SelectList LoadSelectPublicationType(int id = 1)
        {
            var list = manager.GetByFunction<PublicationType>(a => a.IsValid())
                .Select(a => newSelectListItem(a.Name, a.Id, id));
            return selectListCreate(list, "Publication.PublicationType");
        }

        public SelectList LoadSelectPublicationForm(int id = 0)
        {
            var list = publicationElements.Forms.Select(a => newSelectListItem(a.Name, a.Id, id));
            return selectListCreate(list, "Publication.PublicationForm");
        }

        private SelectListItem newSelectListItem(string str, int id, int selectId)
        {
            return new SelectListItem
            {
                Value = id,
                Text = str,
                Selected = (selectId != 0 && selectId.Equals(id) ? true : false)
            };
        }

        private SelectList selectListCreate(IEnumerable<SelectListItem> list, string name)
        {
            var selectList = new ResearchModule.Models.SelectList();
            selectList.AddRange(list);
            selectList.SetName(name);
            return selectList;
        }

        #endregion
    }
}
