using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        public PublicationFilterViewModel PublicationFilterViewModel { get; set; }

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
        public PublicationsViewModel(PublicationService publicationService, AuthorService authorService,
            IEnumerable<Publication> publications, PageInfo pageInfo, int? authorId)
        {
            if (publications.Any())
            {
                this.Publications = publications
                    .Select(a => new PublicationViewModel(publicationService, authorService, a, authorId))
                    .ToList();
                this.PageInfo = pageInfo;
            }
        }


    }

    public class PublicationViewModel
    {
        public Publication Publication { get; set; }
        public IEnumerable<Author> Authors { get; set; }
        public bool IsCurrentAuthor { get; set; }

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

        public PublicationViewModel(PublicationService publicationService, AuthorService authorService, Publication publication, int? authorId) 
            : this(publicationService, authorService, publication)
        {
            AddCurrentAuthor(Publication, authorId);
            
        }

        public void Create(Publication publication)
        {
            this.Publication = publication;
            this.Authors = authorService.GetAuthors(publication.Id);
        }

        public void AddCurrentAuthor(Publication publication, int? authorId)
        {
            if (authorId.HasValue) { 
                IsCurrentAuthor = Authors.Where(a => a.Id == authorId).Any();
            }
            else
            {
                IsCurrentAuthor = false;
            }
        }
    }
}


