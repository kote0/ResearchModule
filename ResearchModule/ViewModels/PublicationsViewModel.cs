using Microsoft.AspNetCore.Http;
using ResearchModule.Components.Models;
using ResearchModule.Models;
using ResearchModule.Service;
using System.Collections.Generic;
using System.Linq;


namespace ResearchModule.ViewModels
{
    public class PublicationsViewModel
    {
        public IEnumerable<PublicationViewModel> Publications { get; private set; }
        public PageInfo PageInfo { get; set; }
        

        public PublicationsViewModel(PublicationService publicationService, AuthorService authorService, 
            IEnumerable<Publication> publications, PageInfo pageInfo)
        {
            if (publications.Any())
            {
                this.Publications = publications
                    .Select(a => new PublicationViewModel(publicationService, authorService,  a))
                    .ToList();
                this.PageInfo = pageInfo;
            }
        }
        
    }

    public class PublicationViewModel
    {
        public Publication Publication { get; set; }
        public IEnumerable<Author> Authors { get; set; }

        private PublicationService publicationService;
        private AuthorService authorService;

        public PublicationViewModel()
        {
        }

        public PublicationViewModel(PublicationService publicationService, AuthorService authorService, Publication publication)
        {
            this.publicationService = publicationService;
            this.authorService = authorService;
            Create(publication);
        }

        public void Create(Publication publication)
        {
            this.Publication = publication;
            this.Authors = authorService.GetAuthors(publication.Id);
        }
    }
}


