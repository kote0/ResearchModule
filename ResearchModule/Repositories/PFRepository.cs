using ResearchModule.Models;
using ResearchModule.Repository.Abstarcts;
using ResearchModule.Repository.Interfaces;

namespace ResearchModule.Repositories
{
    public class PFRepository : PMAbstract<PF, PublicationFilters>
    {
        public PFRepository(IBaseRepository manager) : base(manager)
        {

        }

        public override void AddProperty(ref PublicationFilters filter, PF pm)
        {
        }

        public override void AddProperty(ref PF pm, PublicationFilters multiple)
        {
        }
    }
}
