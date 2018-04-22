﻿using ResearchModule.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public class PublicationFilters : IName
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<PF> PFs { get; set; }
    }
}