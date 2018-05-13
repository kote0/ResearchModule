using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ResearchModule.Components.Models.Interfaces;
using ResearchModule.Data;
using ResearchModule.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResearchModule.Repository.Interfaces
{
    public interface IBaseRepository : ICRUDRepository
    {
        long LongCount<T>(Expression<Func<T, bool>> func) where T : class;
        long LongCount<T>() where T : class;

        T First<T>(Func<T, bool> func) where T : class;
        T Single<T>(Expression<Func<T, bool>> func) where T : class;
        T Get<T>(params object[] keyValues) where T : class;
        IEnumerable<T> Get<T>(Func<T, bool> func) where T : class;
        
        IAsyncEnumerable<T> GetAsync<T>(Func<T, bool> func) where T : class;

        IQueryable<T> GetQuery<T>(Expression<Func<T, bool>> func) where T : class;
        IQueryable<T> GetAll<T>() where T : class;
        IQueryable<T> Page<T>(int page, int pageSize = 10) where T : class;
        IQueryable<T> Page<T>(IQueryable<T> list, int page, int pageSize = 10) where T : class;
    }
}
