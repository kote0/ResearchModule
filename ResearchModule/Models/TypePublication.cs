using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class TypePublication
    {
        public long Id { get; set; }

        public string TypePublicationName { get; set; }

        public virtual ICollection<Publication> Publication { get; set; }
    }
}
