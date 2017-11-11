using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class FormWork
    {
        public long Id { get; set; }
        [Required]
        public string FormName { get; set; }
        public string ShortName { get; set; }

        public virtual ICollection<Publication> Publication { get; set; }

        public bool IsValid()
        {
            if (FormName != null)
                return true;
            return false;
        }
    }
}
