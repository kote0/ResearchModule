using ResearchModule.Components.Models;
using ResearchModule.Managers;
using ResearchModule.Managers.Interfaces;
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
    
        private PublicationService publicationService;

        public PublicationViewModel(PublicationService publicationService, Publication publication)
        {
            this.publicationService = publicationService;
            Create(publication);
        }

        private async void Create(Publication publication)
        {
            this.Publication = publication;
            this.Authors = await publicationService.GetAuthors(publication.Id);
        }
    }
}
