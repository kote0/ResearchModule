﻿using Microsoft.EntityFrameworkCore;
using ResearchModule.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace ResearchModule.Managers
{
    public class BaseManager : IBaseManager, IDisposable 
    {
        public readonly DBContext _db;

        public BaseManager()
        {
            _db = new DBContext();
        }

        public void Create<T>(T record)
        {
            var t = record as EntityEntry;
            if (t.Entity == null) return;
            _db.Entry(t.Entity).State = EntityState.Added;
            _db.SaveChanges();
        }

        public void Delete<T>(long id) where T : class
        {
            var record = _db.Find<T>(id);

            if (record != null)
            {
                _db.Remove(record);
                _db.SaveChanges();
            }
        }

        public void Delete<T>(T record)
        {
            var t = record as EntityEntry;
            if (t.Entity != null)
            {
                _db.Remove(t.Entity);
                _db.SaveChanges();
            }
        }

        public T Get<T>(params object[] keyValues) where T : class
        {
            if (keyValues!= null)
                return _db.Find<T>(keyValues);
            return null;
        }

        public void Update<T>(T record)
        {
            var t = record as EntityEntry;
            if (t.Entity != null)
            {
                _db.Attach(t.Entity).State = EntityState.Modified;
            }
        }

        public IEnumerable<T> GetByFunction<T>(Func<T, bool> func) where T : class
        {
            var list = _db.Set<T>().Where(func);
            return list;
        }

        public List<T> GetAll<T>() where T : class 
        {
            return _db.Set<T>().ToList();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                disposedValue = true;
            }
        }

       
        ~BaseManager()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(false);
        }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        void IDisposable.Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
