using Microsoft.AspNetCore.Http;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.ViewModels
{
    public class CreatePublicationViewModel
    {
        [Required]
        public Publication Publication { get; set; }

        [Required]
        public IFormFile FormFile { get; set; }

        //public IEnumerable<PublicationType> PublicationTypes { get; set; }
        
        public IEnumerable<Author> Authors { get; set; }

        public SelectList PublicationTypes { get; set; }

        public CreatePublicationViewModel()
        {
            //PublicationTypes = new List<PublicationType>();
            Authors = new List<Author>();
        }
    }
}
