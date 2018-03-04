using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class User : IdentityUser
    {
        public Author Author { get; set; }
        public bool IsDeleted { get; set; }
    }
}
