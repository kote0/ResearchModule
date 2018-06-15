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
        public IEnumerable<PublicationViewModel> Publications { get; set; }
        public PageInfo PageInfo { get; set; }
        public PublicationFilterViewModel PublicationFilter { get; set; }

        public PublicationsViewModel()
        {
        }

    }

    public class PublicationViewModel
    {
        public Publication Publication { get; set; }
        public IEnumerable<Author> Authors { get; set; }
        public bool IsCurrentAuthor { get; set; }

        public PublicationViewModel()
        {
        }
    }
}


