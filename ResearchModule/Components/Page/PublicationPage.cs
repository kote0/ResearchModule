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
        private readonly PublicationService publicationService;
        private SelectListService selectListService;
        private readonly Author author;

        public PublicationPage(PublicationManager manager, AuthorService authorService, 
            PublicationService publicationService, Author author, SelectListService selectListService)
        {
            this.manager = manager;
            this.authorService = authorService;
            this.publicationService = publicationService;
            this.author = author;
            this.selectListService = selectListService;
        }

        public object CreatePagination(int first, string action, string controller, string dataId = null)
        {
            var list = manager.Page(first);
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
            model.Authors = authorService.GetAuthors(id);
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
                    var publicationList = new List<Publication>();
                    if (filter.Authors.Count != 0)
                    {
                        foreach (var pub in publications)
                        {
                            var authors = authorService.GetAuthors(pub.Id);
                            if (filter.Authors.Count(au => authors.Any(p => p.Id == au)) > 0)
                            {
                                publicationList.Add(pub);
                            }
                        }
                        publications = publicationList;
                    }
                    

                    var page = new PageInfo(first, listFilters.LongCount());
                    page.SetUrl(action, controller);
                    if (dataId != null)
                        page.DataId = dataId;

                    var view = new PublicationsViewModel();

                    if (publications.Any())
                    {
                        view.Publications = publications.ToList()
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
