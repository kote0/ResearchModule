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
        public IEnumerable<PublicationViewModel> Publications { get; }
        public PageInfo PageInfo { get; set; }

        private readonly BaseManager manager;
        private readonly PublicationService publicationService;

        public PublicationsViewModel() {}

        public PublicationsViewModel(BaseManager manager, PublicationService publicationService)
        {
            this.manager = manager;
            this.publicationService = publicationService;
        }

        public PublicationsViewModel(IQueryable<Publication> Publications)
        {
            if (Publications.Any())
            {
                this.Publications = Publications.Select(a => new PublicationViewModel(a)).ToList();
            }
        }

        
    }
}


