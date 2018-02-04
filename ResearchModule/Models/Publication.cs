using ResearchModule.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public partial class Publication
    {
        private BaseManager manager = new BaseManager(); 

        public long Id { get; set; }

        [Required]
        public string   PublicationName { get; set; }
        public long     PublicationType { get; set; }
        public long     PublicationPartition { get; set; }
        public long     PublicationForm { get; set; }
        public string   TranslateText { get; set; }
        //public long Language { get; set; }


    }

    

}
