using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class PFManager : BaseManager
    {
        public void Create(IEnumerable<PublicationFilter> filters, long pid)
        {
            foreach (var filter in filters)
            {
                PF pf = new PF();
                pf.PublicationFilterId = filter.Id;
                pf.PublicationId = pid;
                Create(pf);
            }
        }
        public void Create(long fid, long pid)
        {
            PF pf = new PF(pid, fid);
            Create(pf);
        }
        public IEnumerable<PublicationFilter> FindFiltersByPublication(long idPublication)
        {
            var pfs = GetByFunction<PF>(pf => pf.PublicationId == idPublication);
            List<PublicationFilter> filters = new List<PublicationFilter>();
            foreach (var item in pfs)
            {
                var filter = GetByFunction<PublicationFilter>(a => a.Id == item.PublicationFilterId).FirstOrDefault();
                if (filter != null)
                {
                    filters.Add(filter);
                }
            }
            return filters;
        }
    }
}
