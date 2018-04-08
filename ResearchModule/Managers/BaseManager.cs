using Microsoft.EntityFrameworkCore;
using ResearchModule.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using ResearchModule.Managers.Interfaces;

namespace ResearchModule.Managers
{
    public class BaseManager : IBaseManager, IDisposable
    {
        public readonly DBContext _db;
        public bool Success { get; protected set; }

        public static IBaseManager Manager
        {
            get { return Locator.GetService<IBaseManager>(); }
        }

        public BaseManager()
        {
            _db = new DBContext();
        }

        public void Create<T>(T record) where T : class
        {
            Success = false;
            try
            {
                if (record == null) return;
                _db.Entry(record).State = EntityState.Added;
                _db.SaveChanges();
                Success = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void Delete<T>(int id) where T : class
        {
            var record = Get<T>(id);
            Delete(record);
        }

        public void Delete<T>(T record) where T : class
        {
            Success = false;
            try
            {
                if (record != null)
                {
                    _db.Remove(record);
                    _db.SaveChanges();
                    Success = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public T Get<T>(params object[] keyValues) where T : class
        {
            if (keyValues != null)
                return _db.Find<T>(keyValues);
            return null;
        }

        public void Update<T>(T record) where T : class
        {
            try
            {
                if (record != null)
                {
                    _db.Attach(record).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public IEnumerable<T> GetByFunction<T>(Func<T, bool> func) where T : class
        {
            return _db.Set<T>().Where(func);
        }

        public async Task<IEnumerable<T>> Get<T>(Func<T, bool> func) where T : class
        {
            return await _db.Set<T>().Where(func).ToAsyncEnumerable().ToList();
        }


        public DbSet<T> DbSet<T>() where T : class
        {
            return _db.Set<T>();
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return _db.Set<T>();
        }

        public IQueryable<T> Page<T>(int page, int pageSize = 10) where T : class
        {
            return _db.Set<T>().Skip((page - 1) * pageSize).Take(pageSize);
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
