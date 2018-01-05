using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class PA
    {
        public long Id { get; set; }
        [Required]
        public long PId { get; set; }
        [Required]
        public long AId { get; set; }

        public PA(long pid, long aid)
        {
            PId = pid;
            AId = aid;
        }
    }
}
