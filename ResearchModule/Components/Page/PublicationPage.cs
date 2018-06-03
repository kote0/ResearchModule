using ResearchModule.Components.Models;
using ResearchModule.Managers;
using ResearchModule.Models;
using ResearchModule.Service;
using ResearchModule.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Page
{
    public class PublicationPage : IPageCreator
    {
        private readonly PublicationManager manager;
        private readonly AuthorService authorService;
        private readonly PublicationService publicationService;
        private readonly Author author;

        public PublicationPage(PublicationManager manager, AuthorService authorService, PublicationService publicationService, UserManager userManager)
        {
            this.manager = manager;
            this.authorService = authorService;
            this.publicationService = publicationService;
            author = userManager.CurrentAuthor();
        }

        public object CreatePagination(int first, string action, string controller, string dataId = null)
        {
            var list = manager.Page(first);
            var count = manager.Count();

            var page = new PageInfo(first, count);
            page.SetUrl(action, controller);
            if (dataId != null)
                page.DataId = dataId;
            return new PublicationsViewModel(publicationService, authorService, list, page, author?.Id);
        }

        public object CreatePagination(object list, int first, string action, string controller, string dataId = null)
        {
            if (list is PublicationFilterViewModel filter)
            {
                if (filter.PublicationTypes == null && string.IsNullOrEmpty(filter.Publication.PublicationName)
                    && string.IsNullOrEmpty(filter.Publication.OutputData))
                {
                    return CreatePagination(first, action, controller, dataId);
                }
                else
                {
                    var listFilters = manager.FilterQuery(filter);
                    var page = new PageInfo(first, listFilters.LongCount());
                    page.SetUrl(action, controller);
                    if (dataId != null)
                        page.DataId = dataId;
                    var publications = listFilters.Page(first).ToList();
                    var view = new PublicationsViewModel(publicationService, authorService, publications, page);
                    view.PublicationFilterViewModel = filter;
                    return view;
                }
            }
            return null;
        }
    }
}
