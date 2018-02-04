using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public partial class PublicationType
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }

        public bool IsValid()
        {
            if (Name != null)
                return true;
            return false;
        }
    }
}
