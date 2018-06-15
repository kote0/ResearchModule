using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models.Interfaces
{
    public interface IName : IID
    {
        string Name { get; set; }
    }
}
