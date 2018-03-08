using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Models
{
    public interface IPublicationElement
    {
        int Id { get; set; }

        string Name { get; set; }

        string Description { get; set; }

        bool Obsolete { get; set; }

        string ShortName { get; set; }
    }
}
