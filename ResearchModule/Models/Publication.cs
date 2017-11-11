﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class Publication
    {
        public long Id { get; set; }

        [Required]
        public string PublicationName { get; set; }
        public long TypePublicationId { get; set; }
        public long SectionId { get; set; }
        public long FormWorkId { get; set; }
        public bool IsTranslate { get; set; }
        public string TranslateText { get; set; }
        public string Language { get; set; }


        public virtual TypePublication TypePublication { get; set; }
        public virtual FormWork FormWork { get; set; }


        public bool IsValid()
        {
            if (PublicationName != null)
                return true;
            return false;
        }
    }
}
