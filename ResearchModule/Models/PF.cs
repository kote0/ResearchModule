using ResearchModule.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    /// <summary>
    /// Публикации : Фильтры
    /// </summary>
    public class PF : IPublicationMultiple<PublicationFilters>
    {
        public Publication Publication { get; set; }

        [Required]
        public int PublicationId { get; set; }

        public PublicationFilters Multiple { get; set; }

        [Required]
        public int MultipleId { get; set; }

        public PF() { }

        public PF(int publicationId, int multilpeId)
        {
            PublicationId = publicationId;
            MultipleId = multilpeId;
        }
    }
}
