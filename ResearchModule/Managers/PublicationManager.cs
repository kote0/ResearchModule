using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class PublicationManager : BaseManager<Publication>
    {
        public override List<Publication> GetAll()
        {
            return _db.Publication.ToList();
        }
    }
}
