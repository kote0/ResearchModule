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
        public Publication Publication { get; private set; }
        public IEnumerable<Author> Authors { get; private set; }

        private BaseManager manager;
        private PublicationService publicationService;

        public PublicationViewModel() { }

        public PublicationViewModel(BaseManager manager, PublicationService publicationService)
        {
            this.manager = manager;
            this.publicationService = publicationService;
        }

        public async void Create(Publication publication)
        {
            this.Publication = publication;
            this.Authors = await publicationService.GetAuthors(publication.Id);
        }
    }
}
