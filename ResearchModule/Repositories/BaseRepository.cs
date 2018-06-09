using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using ResearchModule.Components.Models;
using ResearchModule.Components.Models.Interfaces;
using ResearchModule.Data;
using ResearchModule.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResearchModule.Repository
{
    public class BaseRepository : IBaseRepository, IDisposable
    {
        public DBContext _db { get; }
        public bool Success { get; protected set; }

        private ILogger logger;
        private static readonly Lazy<IBaseRepository> _instance =
            new Lazy<IBaseRepository>(() => Locator.GetService<IBaseRepository>());

        public static IBaseRepository Instance
        {
            get { return _instance.Value; }
        }

        public BaseRepository(ILogger<BaseRepository> logger)
        {
            _db = new DBContext();
            this.logger = logger;
        }

        public long LongCount<T>(Expression<Func<T, bool>> func) where T : class
        {
            return _db.Set<T>().LongCount(func);
        }

        public long LongCount<T>() where T : class
        {
            return _db.Set<T>().LongCount();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public IResult Add<T>(T record) where T : class
        {
            var result = new Result();
            try
            {
                if (record == null) return result.Set("{0} is null", record);
                _db.Add(record);
                //_db.SaveChanges();
                result.Model = record;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Format("Ошибка при создании записи {0}", record));
                result.Set(string.Format("Error: {1} Ошибка при создании записи {0}.", record, ex));
                throw;
            }
            return result;
        }

        public IResult Delete<T>(int id) where T : class
        {
            return Delete(Get<T>(id));
        }

        public IResult Delete<T>(T record) where T : class
        {
            var result = new Result();
            try
            {
                if (record == null) return result.Set("{0} is null", record);
                //_db.Entry(record).State = EntityState.Deleted;
                _db.Remove(record);
                //_db.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Format("Ошибка при удалении записи {0}", record));
                throw;
            }
            return result;
        }

        public IResult DeleteRange<T>(IEnumerable<T> records) where T : class
        {
            var result = new Result();
            try
            {
                if (records == null && records.Count() != 0) return result.Set("{0} is null", records);
                _db.RemoveRange(records);
                ////_db.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Format("Ошибка при удалении записи {0}", records));
                throw;
            }
            return result;
        }

        public T Get<T>(params object[] keyValues) where T : class
        {
            if (keyValues != null)
                return _db.Find<T>(keyValues);
            return null;
        }

        public IResult Update<T>(T record) where T : class
        {
            var result = new Result();
            try
            {
                if (record == null) return result.Set("{0} is null", record);
                _db.Update(record);
                //_db.Entry(record).State = EntityState.Modified;
                //_db.SaveChanges();
                result.Model = record;
            }
            catch (Exception ex)
            {
                result.Set(string.Format("Error: {1} Ошибка при изменении записи {0}.", record, ex));
                logger.LogError(ex, string.Format("Ошибка при изменении записи {0}", record));
                throw;
            }
            return result;
        }

        public IResult UpdateRange(IEnumerable<object> records)
        {
            var result = new Result();
            try
            {
                if (records.Count() == 0)
                    return result.Set("Нет записей для изменения {0}", records);
                _db.UpdateRange(records);
                //_db.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Format("Ошибка при изменении записей {0}", records));
                throw;
            }
            return result;
        }

        public IResult UpdateRange(params object[] records)
        {
            var result = new Result();
            try
            {
                if (records.Count() == 0)
                    return result.Set("Нет записей для изменения {0}", records);
                _db.UpdateRange(records);
                //_db.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Format("Ошибка при изменении записей {0}", records));
                throw;
            }
            return result;
        }


        public IResult AddRange(IEnumerable<object> records)
        {
            var result = new Result();
            try
            {
                if (records.Count() == 0)
                    return result.Set("Нет записей {0}", records);
                _db.AddRange(records);
                //_db.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Format("Ошибка при создании записей {0}", records));
                throw;
            }
            return result;
        }

        public IResult AddRange(params object[] records)
        {
            var result = new Result();
            try
            {
                if (records.Count() == 0)
                    return result.Set("Нет записей {0}", records);
                _db.AddRange(records);
                //_db.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Format("Ошибка при создании записей {0}", records));
            }
            return result;
        }

        public IEnumerable<T> Get<T>(Func<T, bool> func) where T : class
        {
            return _db.Set<T>().Where(func);
        }

        public IQueryable<T> GetQuery<T>(Expression<Func<T, bool>> func) where T : class
        {
            return _db.Set<T>().Where(func);
        }

        //Action<Publication>
        public Task ForEachAsync<T>(Action<T> func) where T : class
        {
            return _db.Set<T>().ForEachAsync(func);
        }

        public IAsyncEnumerable<T> GetAsync<T>(Func<T, bool> func) where T : class
        {
            return _db.Set<T>().Where(func).ToAsyncEnumerable();
        }

        public T First<T>(Func<T, bool> func) where T : class
        {
            return _db.Set<T>().FirstOrDefault(func);
        }

        public T Single<T>(Expression<Func<T, bool>> func) where T : class
        {
            return _db.Set<T>().SingleOrDefault(func);
        }


        public DbSet<T> Set<T>() where T : class
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

        public IQueryable<T> Page<T>(IQueryable<T> list, int page, int pageSize = 10) where T : class
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public void SQL(string str)
        {
            _db.Database.ExecuteSqlCommand(str);
        }

        public IIncludableQueryable<T, TProperty> Include<T, TProperty>(Expression<Func<T, TProperty>> func, DbSet<T> type = null) where T : class
        {
            if (type == null) type = Set<T>();

            return type.Include(func);
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


        ~BaseRepository()
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

