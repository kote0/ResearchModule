using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class TypePublication
    {
        public long Id { get; set; }
        [Required]
        public string TypePublicationName { get; set; }

        public virtual ICollection<Publication> Publication { get; set; }


        public bool IsValid()
        {
            if (TypePublicationName != null)
                return true;
            return false;
        }
    }
}
