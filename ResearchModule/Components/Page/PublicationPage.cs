using ResearchModule.Components.Models;
using ResearchModule.Extensions;
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
        private readonly Author author;

        public PublicationPage(PublicationManager manager, AuthorService authorService, UserManager userManager)
        {
            this.manager = manager;
            this.authorService = authorService;
            this.author = userManager.CurrentAuthor();
        }

        public object CreatePagination(int first, string action, string controller, string dataId = null)
        {
            var list = manager.LoadWithAuthors().Page(first);
            var count = manager.Count();
            var page = new PageInfo(first, count);
            page.SetUrl(action, controller);
            if (dataId != null)
                page.DataId = dataId;
            var view = new PublicationsViewModel();
            view.Publications = list.ToList()
                .Select(a => AppendPublication(a));
            view.PageInfo = page;
            return view;
        }

        private PublicationViewModel AppendPublication(Publication publication)
        {
            var model = new PublicationViewModel();
            model.Publication = publication;
            var id = publication.Id;
            model.Authors = publication.PAs.Count != 0 ? publication.PAs.Select(a => a.Multiple) : null; 
            model.IsCurrentAuthor = (author != null) 
                ? author.IsAdmin() 
                    ? true 
                    : model.Authors.Where(a => a.Id == author.Id).Any() 
                : false;
            return model;
        }

        public object CreatePagination(object list, int first, string action, string controller, string dataId = null)
        {
            if (list is PublicationFilterViewModel filter)
            {
                if (filter.PublicationTypesId == null && string.IsNullOrEmpty(filter.Publication.PublicationName)
                    && string.IsNullOrEmpty(filter.Publication.OutputData))
                {
                    return CreatePagination(first, action, controller, dataId);
                }
                else
                {
                    var listFilters = manager.FilterQuery(filter);

                    var publications = listFilters.Page(first).ToList();
                    

                    var page = new PageInfo(first, listFilters.LongCount());
                    page.SetUrl(action, controller);
                    if (dataId != null)
                        page.DataId = dataId;

                    var view = new PublicationsViewModel();

                    if (publications.Any())
                    {
                        view.Publications = publications
                            .Select(a => AppendPublication(a));
                        view.PageInfo = page;
                    }
                    view.PublicationFilter = filter;
                    return view;
                }
            }
            return null;
        }
    }
}
