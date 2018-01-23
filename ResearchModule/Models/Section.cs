using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public partial class Section
    {
        public long Id { get; set; }
        [Required]
        public string SectionName { get; set; }

        public List<Publication> Publication { get; set; }

        public bool IsValid()
        {
            if (SectionName != null)
                return true;
            return false;
        }
    }
}
