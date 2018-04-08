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
    public class PublicationsViewModel
    {
        public IEnumerable<PublicationViewModel> Publications { get; private set; }
        public PageInfo PageInfo { get; set; }
        

        public PublicationsViewModel(PublicationService publicationService, 
            IEnumerable<Publication> publications, PageInfo pageInfo)
        {
            if (publications.Any())
            {
                this.Publications = publications
                    .Select(a => new PublicationViewModel(publicationService, a))
                    .ToList();
                this.PageInfo = pageInfo;
            }
        }
        
    }
}


