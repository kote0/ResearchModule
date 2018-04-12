using Microsoft.AspNetCore.Http;
using ResearchModule.Models;
using ResearchModule.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models
{
    public class FileDetail : IID
    {
        public int Id { get; set; }

        public string Uid { get; set; }
        
        public string Name { get; set; }
        
        [NotMapped]
        public IFormFile FormFile { get; set; }
        
        public long Size { get; set; }

        public Publication Publication { get; set; }
    }
}
