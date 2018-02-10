using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models
{
    public class FileDetails
    {

        public string Uid { get; set; }
        
        public string Name { get; set; }
        
        public IFormFile FormFile { get; set; }
        
        public long Size { get; set; }
    }
}
