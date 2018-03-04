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

        public PublicationType PublicationType { get; set; }   

        public IEnumerable<Author> Author { get; set; }

    }
}
