using System.Collections.Generic;
using System.Linq;
using ResearchModule.Models;
using ResearchModule.Extensions;
using ResearchModule.Repository.Interfaces;
using ResearchModule.Models.Interfaces;

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

        public SelectListService()
        {
        }

        public SelectList Create<T>(int? id) where T : class, IName
        {
            return Create<T>(manager.GetQuery<T>(p => p != null), id);
        }

        public SelectList Create<T>(IQueryable<T> list, int? id) where T : class, IName
        {
            return Create(list.ToList(), id);
        }

        public SelectList Create<T>(IEnumerable<T> list, int? id) where T : class, IName
        {
            var selectedId = id.HasValue ? id.Value : 0;
            return selectListCreate(list.Select(a => CreateItem(a.Name, a.Id, selectedId)));
        }

        public SelectList Create<T>(IEnumerable<int> id) where T : class, IName
        {
            return selectListCreate(manager.GetQuery<T>(p => p != null)
                .ToList()
                .Select(a => CreateItem(a.Name, a.Id, id)));
        }

        public SelectList Create<T>(IEnumerable<T> list, IEnumerable<int> id) where T : class, IName
        {
            return selectListCreate(list.Select(a => CreateItem(a.Name, a.Id, id)));
        }


        public SelectList LoadSelectAuthor(int id = 0)
        {
            var list =  manager.Get<Author>(a => a.IsValid()) //сделать асинхронным
                .Select(a => CreateItem(a.ToStringFormat(),a.Id, id));
            return selectListCreate(list, "Author");
        }

        public SelectList LoadSelectPublicationType(int id = 1)
        {
            return LoadSelectPublicationType("Publication.PublicationTypeId", id);
        }

        public SelectList LoadSelectPublicationType(string name, int id = 1)
        {
            var list = manager.GetQuery<PublicationType>(a => a.IsValid())
                .Select(a => CreateItem(a.Name, a.Id, id));
            return selectListCreate(list.ToList(), name);
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

        private SelectListItem CreateItem(string str, int id, IEnumerable<int> ids)
        {
            return new SelectListItem
            {
                Value = id,
                Text = str,
                Selected = (ids.Count() != 0 && ids.Count(a => a.Equals(id)) > 0 ? true : false)
            };
        }

        private SelectList selectListCreate(IEnumerable<SelectListItem> list, string name)
        {
            var selectList = new ResearchModule.Models.SelectList();
            selectList.AddRange(list);
            selectList.SetName(name);
            return selectList;
        }

        private SelectList selectListCreate(IEnumerable<SelectListItem> list)
        {
            var selectList = new ResearchModule.Models.SelectList();
            selectList.AddRange(list);
            return selectList;
        }
        #endregion
    }
}
