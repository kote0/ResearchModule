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
    public class PublicationsViewModel
    {
        public IEnumerable<PublicationViewModel> Publications { get; private set; }
        public PageInfo PageInfo { get; set; }

        private readonly BaseManager manager;
        private readonly PublicationService publicationService;

        public PublicationsViewModel(BaseManager manager, PublicationService publicationService)
        {
            this.manager = manager;
            this.publicationService = publicationService;
        }


        public void Create(IQueryable<Publication> Publications)
        {
            if (Publications.Any())
            {
                this.Publications = Publications.Select(a => CreatePublication(a)).ToList();
            }
        }

        private PublicationViewModel CreatePublication(Publication publication)
        {
            var newPublication = new PublicationViewModel(manager, publicationService);
            newPublication.Create(publication);
            return newPublication;
        }
        
    }
}


