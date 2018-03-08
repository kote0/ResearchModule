﻿using System;
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
                .Select(a =>
                    new ResearchModule.Models.SelectListItem
                    {
                        Value = a.Id,
                        Text = string.Format("{0}.{1}. {2}", a.Surname.Substring(0, 1), a.Lastname.Substring(0, 1), a.Name),
                        Selected = (id != 0 && id == a.Id ? true : false)
                    })
                .ToList();

            return selectListCreate(list, "Author");
        }


        public SelectList LoadSelectPublicationPartition(int id = 0)
        {

            var list = publicationElements.Partitions.Where(p => !p.Obsolete)
                .Select(a =>
                    new ResearchModule.Models.SelectListItem
                    {
                        Value = a.Id,
                        Text = a.Name,
                        Selected = (id != 0 && id == a.Id ? true : false)
                    })
                .ToList();
            return selectListCreate(list, "Publication.PublicationPartition");
        }

        public SelectList LoadSelectPublicationType(int id = 0)
        {
            var list = manager.GetByFunction<PublicationType>(a => a.IsValid())
                .Select(a =>
                    new ResearchModule.Models.SelectListItem
                    {
                        Value = a.Id,
                        Text = a.Name,
                        Selected = (id != 0 && id == a.Id ? true : false)
                    })
                .ToList();
            return selectListCreate(list, "Publication.PublicationType");
        }

        public SelectList LoadSelectPublicationForm(int id = 0)
        {
            var list = publicationElements.Forms.Where(f => !f.Obsolete)
                .Select(a =>
                        new ResearchModule.Models.SelectListItem
                        {
                            Value = a.Id,
                            Text = a.Name,
                            Selected = (id != 0 && id.Equals(a.Id) ? true : false)
                        })
                .ToList();
            return selectListCreate(list, "Publication.PublicationForm");
        }

        private ResearchModule.Models.SelectList selectListCreate(List<ResearchModule.Models.SelectListItem> list, string name)
        {
            var selectList = new ResearchModule.Models.SelectList();
            if (list.Count != 0)
                selectList.AddRange(list);

            selectList.SetName(name);
            return selectList;
        }

        #endregion
    }
}
