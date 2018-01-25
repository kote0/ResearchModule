using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public interface IBaseManager
    {
        //bool Create<T>(T record); 

        T Get<T>(params object[] keyValues) where T:class;

        //bool Update<T>(T record);
        
        //bool Delete<T>(T record);

        void Delete<T>(long id) where T : class;

        IEnumerable<T> GetByFunction<T>(Func<T, bool> func) where T:class;

        List<T> GetAll<T>() where T:class;
        
    }
}
