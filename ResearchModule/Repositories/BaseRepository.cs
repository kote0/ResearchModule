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

        public void Save()
        {
            _db.SaveChanges();
        }

        public IResult Create<T>(T record) where T : class
        {
            var result = new Result();
            try
            {
                if (record == null) return result.Set("{0} is null", record);
                _db.Add(record);
                //_db.Entry(record).State = EntityState.Added;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Format("Ошибка при создании записи {0}", record));
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
                _db.Remove(record);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Format("Ошибка при удалении записи {0}", record));
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
                _db.Entry(record).State = EntityState.Modified;
                //_db.Update(record);
                //_db.Attach(record).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
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
                _db.SaveChanges();
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
                _db.SaveChanges();
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
                _db.SaveChanges();
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
                _db.SaveChanges();
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

        public IAsyncEnumerable<T> GetAsync<T>(Func<T, bool> func) where T : class
        {
            return _db.Set<T>().Where(func).ToAsyncEnumerable();
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

//public void CreateOrUpdate<T>(T record) where T : class
//{
//    _db.ChangeTracker.TrackGraph(record, node =>r(node));

//    var entry = _db.Entry(record);
//    switch (entry.State)
//    {
//        case EntityState.Detached:
//            _db.Add(record);
//            break;
//        case EntityState.Modified:
//            _db.Update(record);
//            break;
//        case EntityState.Added:
//            _db.Add(record);
//            break;
//        case EntityState.Unchanged:
//            //item already in db no need to do anything  
//            break;

//        default:
//            throw new ArgumentOutOfRangeException();
//    }
//}

//private void r(EntityEntryGraphNode node)
//{
//    var entry = node.Entry;

//    if ((int)entry.Property("Id").CurrentValue < 0)
//    {
//        entry.State = EntityState.Added;
//        entry.Property("Id").IsTemporary = true;
//    }
//    else
//    {
//        entry.State = EntityState.Modified;
//    }
//}