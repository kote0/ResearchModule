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

        public List<PublicationType> PublicationType { get; set; }

    }
}
