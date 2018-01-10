using Microsoft.EntityFrameworkCore;
using ResearchModule.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers
{
    public class BaseManager<T> : IBaseManager<T>, IDisposable where T : class
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
            return _db.Find<T>(id);
        }

        public void Update(T record)
        {
            _db.Attach(record).State = EntityState.Modified;
        }

        public IEnumerable<T> GetByFunction(Func<T, bool> func)
        {
            var list = _db.Set<T>().Where(func);
            return list;
        }

        public List<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        public BaseManager<T> Set<TEntity>()
        {
            return new BaseManager<T>();
        }

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
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
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
