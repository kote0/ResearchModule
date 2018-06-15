using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Service
{
    public static class ListService
    {
        //used by LINQ to SQL
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize = 10)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        //used by LINQ
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize = 10)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static IQueryable<TSource> Page<TSource, TProperty>(this IIncludableQueryable<TSource, TProperty> source, int page, int pageSize = 10)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
