using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class PF
    {
        public int Id { get; set; }
        [Required]
        public int PublicationId { get; set; }
        public virtual Publication Publication { get; set; }
        [Required]
        public int PublicationFilterId { get; set; }
        public virtual PublicationFilters PublicationFilter { get; set; }

        public PF() { }
        public PF(int publicationId, int publicationFilterId)
        {
            PublicationId = publicationId;
            PublicationFilterId = publicationFilterId;
        }
    }
}
