using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class Author
    {
        public long Id { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool CoAuthor { get; set; }

        public virtual ICollection<Publication> Publication { get; set; }

        public bool IsValid()
        {
            if (!(string.IsNullOrEmpty(Surname) && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(LastName)))
                return true;
            return false;
        }
    }
}
