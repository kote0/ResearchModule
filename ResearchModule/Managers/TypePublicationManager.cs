using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class TypePublicationManager : BaseManager<TypePublication>
    {
        public override List<TypePublication> GetAll()
        {
            return _db.TypePublication.ToList();
        }
    }
}
