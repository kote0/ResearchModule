using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class Author
    {
        public long Id { get; set; }

        public string Surname { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool CoAuthor { get; set; }

        public virtual ICollection<Publication> Publication { get; set; }
    }
}
