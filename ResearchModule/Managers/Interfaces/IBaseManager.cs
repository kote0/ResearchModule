using Microsoft.EntityFrameworkCore;
using ResearchModule.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Managers.Interfaces
{

    public interface IBaseManager 
    {
        void Create<T>(T record) where T : class;
        void Delete<T>(int id) where T : class;
        void Delete<T>(T record) where T : class;
        T Get<T>(params object[] keyValues) where T : class;
        void Update<T>(T record) where T : class;
        IEnumerable<T> GetByFunction<T>(Func<T, bool> func) where T : class;
        Task<IEnumerable<T>> Get<T>(Func<T, bool> func) where T : class;
        DbSet<T> DbSet<T>() where T : class;
        IQueryable<T> GetAll<T>() where T : class;
        IQueryable<T> Page<T>(int page, int pageSize = 10) where T : class;
    }
}
