using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public interface IBaseManager<T>
    {
        void Create(T record);

        T Get(long? id);

        void Update(T record);

        void Delete(T record);

        void Delete(long? id);
    }
}
