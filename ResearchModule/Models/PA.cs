using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public partial class PA
    {
        public long Id { get; set; }
        [Required]
        public long PId { get; set; }
        [Required]
        public long AId { get; set; }

        /// <summary>
        /// Вес автора в публикации
        /// </summary>
        public double Weight { get; set; }

        public PA() { }
        public PA(long pid, long aid)
        {
            PId = pid;
            AId = aid;
        }
    }
}
