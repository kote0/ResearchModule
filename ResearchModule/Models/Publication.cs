using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class Publication
    {
        public long Id { get; set; }

        public string PublicationName { get; set; }
        public long TypePublicationId { get; set; }
        public long AuthorId { get; set; }
        public long FormWorkId { get; set; }

        public virtual Author Author { get; set; }
        public virtual TypePublication TypePublication { get; set; }
        public virtual FormWork FormWork { get; set; }
    }
}
