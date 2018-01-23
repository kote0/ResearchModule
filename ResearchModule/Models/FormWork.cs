using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public partial class FormWork
    {
        public FormWork()
        {
            Publication = new HashSet<Publication>();
        }

        public long Id { get; set; }
        [Required]
        public string FormName { get; set; }
        public string ShortName { get; set; }

        public ICollection<Publication> Publication { get; set; }

        public bool IsValid()
        {
            if (FormName != null)
                return true;
            return false;
        }

        public string ToStringFormat()
        {
            if (!(string.IsNullOrEmpty(FormName) && string.IsNullOrEmpty(ShortName)))
                return string.Format("{0}({1}.)", FormName, ShortName);
            return null;
        }
    }
}
