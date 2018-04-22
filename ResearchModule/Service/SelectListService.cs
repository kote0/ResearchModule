using System.Collections.Generic;
using System.Linq;
using ResearchModule.Models;
using ResearchModule.Extensions;
using ResearchModule.Repository.Interfaces;

namespace ResearchModule.Service
{
    public class SelectListService
    {
        private readonly IBaseRepository manager;
        private readonly PublicationElements publicationElements;

        public SelectListService(IBaseRepository manager, PublicationElements publicationElements)
        {
            this.publicationElements = publicationElements;
            this.manager = manager;
        }

        public SelectList LoadSelectAuthor(int id = 0)
        {
            var list =  manager.Get<Author>(a => a.IsValid()) //сделать асинхронным
                .Select(a => CreateItem(a.ToStringFormat(),a.Id, id));
            return selectListCreate(list, "Author");
        }

        public SelectList LoadSelectPublicationType(int id = 1)
        {
            var list = manager.GetQuery<PublicationType>(a => a.IsValid())
                .Select(a => CreateItem(a.Name, a.Id, id));
            return selectListCreate(list.ToList(), "Publication.PublicationTypeId");
        }

        public SelectList LoadSelectPublicationPartition(int id = 0)
        {
            var list = publicationElements.Partitions
                .Select(a => CreateItem(a.Name, a.Id, id));
            return selectListCreate(list, "Publication.PublicationPartition");
        }

        public SelectList LoadSelectPublicationForm(int id = 0)
        {
            var list = publicationElements.Forms
                .Select(a => CreateItem(a.Name, a.Id, id));
            return selectListCreate(list, "Publication.PublicationForm");
        }

        #region private

        private SelectListItem CreateItem(string str, int id, int selectId)
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
