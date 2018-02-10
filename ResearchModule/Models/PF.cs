using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class PF
    {
        public long Id { get; set; }
        [Required]
        public long PublicationId { get; set; }
        [Required]
        public long PublicationFilterId { get; set; }

        public PF() { }
        public PF(long publicationId, long publicationFilterId)
        {
            PublicationId = publicationId;
            PublicationFilterId = publicationFilterId;
        }
    }
}
