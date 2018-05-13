using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ResearchModule.Components.Models.Interfaces;
using ResearchModule.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResearchModule.Repositories.Interfaces
{
    public interface ICRUDRepository 
    {
        DBContext _db { get; }

        void Save();

        IResult Add<T>(T record) where T : class;
        IResult AddRange(IEnumerable<object> records);
        IResult AddRange(params object[] records);

        IResult Delete<T>(int id) where T : class;
        IResult Delete<T>(T record) where T : class;
        IResult DeleteRange<T>(IEnumerable<T> records) where T : class;

        IResult Update<T>(T record) where T : class;
        IResult UpdateRange(params object[] records);
        IResult UpdateRange(IEnumerable<object> records);

        DbSet<T> Set<T>() where T : class;

        IIncludableQueryable<T, TProperty>
            Include<T, TProperty>(Expression<Func<T, TProperty>> func,
            DbSet<T> type = null)
            where T : class;
    }
}
