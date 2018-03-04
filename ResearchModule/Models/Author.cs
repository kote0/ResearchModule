using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public partial class Author
    {
        public long Id { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public DateTime BDay { get; set; }

        [NotMapped]
        public bool Selected { get; set; }
        [NotMapped]
        public double Weight { get; set; }

        public bool IsValid()
        {
            if (!(string.IsNullOrEmpty(Surname) && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Lastname)))
                return true;
            return false;
        }
    }
}
