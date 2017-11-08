using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class FormWorkManager : BaseManager<FormWork>
    {
        public override List<FormWork> GetAll()
        {
            return _db.FormWork.ToList();
        }
    }
}
