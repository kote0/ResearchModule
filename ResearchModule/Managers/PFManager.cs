using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class PFManager 
    {
        private readonly BaseManager manager;

        public PFManager(BaseManager manager)
        {
            this.manager = manager;
        }

        public void Create(IEnumerable<PublicationFilters> filters, int pid)
        {
            foreach (var filter in filters)
            {
                PF pf = new PF();
                pf.PublicationFilterId = filter.Id;
                pf.PublicationId = pid;
                manager.Create(pf);
            }
        }
        public void Create(int fid, int pid)
        {
            PF pf = new PF(pid, fid);
            manager.Create(pf);
        }
        public IEnumerable<PublicationFilters> FindFiltersByPublication(int idPublication)
        {
            var pfs = manager.GetByFunction<PF>(pf => pf.PublicationId == idPublication);
            List<PublicationFilters> filters = new List<PublicationFilters>();
            foreach (var item in pfs)
            {
                var filter = manager.GetByFunction<PublicationFilters>(a => a.Id == item.PublicationFilterId).FirstOrDefault();
                if (filter != null)
                {
                    filters.Add(filter);
                }
            }
            return filters;
        }
    }
}
