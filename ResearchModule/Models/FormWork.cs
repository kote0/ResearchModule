using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class FormWork
    {
        public long Id { get; set; }

        public string FormName { get; set; }
        public string ShortName { get; set; }

        public virtual ICollection<Publication> Publication { get; set; }
    }
}
