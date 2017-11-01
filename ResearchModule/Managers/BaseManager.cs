using ResearchModule.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class BaseManager<T> : IBaseManager<T> where T : class
    {
        private readonly DBContext _db;

        public BaseManager(DBContext db)
        {
            _db = db;
        }

        public T Create()
        {
            throw new NotImplementedException();
        }

        public void Delete(T table)
        {
            
            throw new NotImplementedException();
        }

        public T Get(long id)
        {
            throw new NotImplementedException();
        }

        public void Update(T table)
        {
            throw new NotImplementedException();
        }
    }
}
