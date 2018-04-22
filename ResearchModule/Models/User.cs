using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class User : IdentityUser
    {        
        public bool IsDeleted { get; set; }

        public Author Author { get; set; }
    }
}
