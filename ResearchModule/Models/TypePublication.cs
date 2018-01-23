using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public partial class TypePublication
    {
        public TypePublication()
        {
            Publication = new HashSet<Publication>();
        }

        public long Id { get; set; }
        [Required]
        public string TypePublicationName { get; set; }

        public ICollection<Publication> Publication { get; set; }


        public bool IsValid()
        {
            if (TypePublicationName != null)
                return true;
            return false;
        }
    }
}
