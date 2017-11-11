using Microsoft.EntityFrameworkCore;
using ResearchModule.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class BaseManager<T> : IBaseManager<T> where T : class
    {
        public readonly DBContext _db;

        public BaseManager()
        {
            _db = new DBContext();
        }

        public void Create(T record)
        {
            if (record == null) return;
            _db.Entry(record).State = EntityState.Added;
            _db.SaveChanges();        
        }

        public void Delete(long? id)
        {
            var record = _db.Find<T>(id);

            if (record != null)
            {
                _db.Remove(record);
                _db.SaveChanges();
            }
        }

        public void Delete(T record)
        {
            if (record != null)
            {
                _db.Remove(record);
                _db.SaveChanges();
            }
        }

        public T Get(long id)
        {
            return (id != null) ? _db.Find<T>(id) : null;
        }

        public void Update(T record)
        {
            _db.Attach(record).State = EntityState.Modified;
        }

        public List<T> GetByFunction(Func<T, bool> func)
        {
            var list = _db.Set<T>().Where(func);
            return list?.ToList();
        }

        public List<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }
    }
}
