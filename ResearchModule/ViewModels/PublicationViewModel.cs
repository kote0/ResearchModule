using ResearchModule.Components.Models;
using ResearchModule.Managers;
using ResearchModule.Models;
using ResearchModule.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.ViewModels
{
    public class PublicationViewModel
    {
        public Publication Publication { get; }
        public IEnumerable<Author> Authors { get; }

        private BaseManager manager;
        private readonly PublicationService publicationService;

        public PublicationViewModel() { }

        public PublicationViewModel(BaseManager manager, PublicationService publicationService)
        {
            this.manager = manager;
            this.publicationService = publicationService;
        }

        public PublicationViewModel(Publication publication)
        {
            this.Publication = publication;
            this.Authors = publicationService.GetAuthors(Publication.Id);
        }
    }
}
