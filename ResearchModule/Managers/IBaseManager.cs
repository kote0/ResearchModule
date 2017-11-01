using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public interface IBaseManager<T>
    {
        T Create();

        T Get(long id);

        void Update(T user);

        void Delete(T user);
    }
}
