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

        [Required(ErrorMessage = "Заполните обязательное поле")]
        public IFormFile FormFile { get; set; }

        public bool TranslateName
        {
            get
            {
                return Publication != null && !string.IsNullOrEmpty(Publication.TranslateText) && Publication.Language.HasValue;
            }
        }

        public string OldFileName { get; set; }

        public IEnumerable<Author> Authors { get; set; }

        public SelectList PublicationTypes { get; set; }

        public SelectList PublicationPartions { get; set; }

        public SelectList PublicationForms { get; set; }

        public CreatePublicationViewModel()
        {
            Authors = new List<Author>();
        }
    }
}
