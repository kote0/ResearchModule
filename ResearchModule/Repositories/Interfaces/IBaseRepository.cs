using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ResearchModule.Components.Models.Interfaces;
using ResearchModule.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResearchModule.Repository.Interfaces
{
    public interface IBaseRepository
    {
        DBContext _db { get; }

        IResult AddRange(IEnumerable<object> records);
        IResult AddRange(params object[] records);

        IResult Create<T>(T record) where T : class;
        //IResult CreateOrUpdate<T>(T record) where T : class;

        IResult Delete<T>(int id) where T : class;
        IResult Delete<T>(T record) where T : class;

        T Get<T>(params object[] keyValues) where T : class;
        IEnumerable<T> Get<T>(Func<T, bool> func) where T : class;
        IQueryable<T> GetQuery<T>(Expression<Func<T, bool>> func) where T : class;
        IAsyncEnumerable<T> GetAsync<T>(Func<T, bool> func) where T : class;
        IQueryable<T> GetAll<T>() where T : class;

        IResult Update<T>(T record) where T : class;
        IResult UpdateRange(params object[] records);
        IResult UpdateRange(IEnumerable<object> records);
        
        DbSet<T> Set<T>() where T : class;
        
        IIncludableQueryable<T, TProperty>
            Include<T, TProperty>(Expression<Func<T, TProperty>> func,
            DbSet<T> type = null)
            where T : class;

        IQueryable<T> Page<T>(int page, int pageSize = 10) where T : class;
    }
}
