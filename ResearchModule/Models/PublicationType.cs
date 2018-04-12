using ResearchModule.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public partial class PublicationType : IName
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(Name))
                return true;
            return false;
        }
    }
}
