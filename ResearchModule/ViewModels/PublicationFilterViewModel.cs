using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.ViewModels
{
    public class PublicationFilterViewModel
    {
        public Publication Publication { get; set; }

        public List<PublicationType> PublicationTypes { get; set; }

        public List<Author> Authors { get; set; }

        public PublicationFilterViewModel()
        {
            Publication = new Publication();
            PublicationTypes = new List<PublicationType>();
            Authors = new List<Author>();
        }
    }
}
