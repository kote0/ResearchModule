using Microsoft.AspNetCore.Mvc;
using ResearchModule.Models;
using ResearchModule.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.ViewModels
{
    public class PublicationFilterViewModel
    {
        public Publication Publication { get; set; }
        
        public List<int> PublicationTypesId { get; set; }

        public SelectList AuthorList { get; set; }

        [DisplayName("Авторы")]
        public List<int> Authors { get; set; }

        public PublicationFilterViewModel()
        {
            Publication = new Publication();
            Authors = new List<int>();
            PublicationTypesId = new List<int>();
        }
    }
}
