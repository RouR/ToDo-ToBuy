using System;
using System.Linq;
using AutoMapper;
using Domain.Interfaces;

namespace Utils.WebRequests
{
    public static class PagingExtensions
    {
        private class DefaultPaginationSetting : IPaginationSetting
        {
            public int Page { get; } = 1; 
            public int PageSize { get; } = 25; 
        }

        public static Pagination<T> AsPagination<T>(this IQueryable<T> query, int pageNumber)
        {
            return AsPagination<T>(query, new DefaultPaginationSetting());
        }

        public static Pagination<T> AsPagination<T>(this IQueryable<T> query, IPaginationSetting settings)
        {
            var results = query.Skip((settings.Page - 1) * settings.PageSize).Take(settings.PageSize);
            return new Pagination<T>(results, query.Count(), settings);
        }
        
        public static Pagination<TK> Map<T, TK>(this Pagination<T> from, Func<T,TK> convert)
        {
            return new Pagination<TK>(from.Items.Select(convert), from.TotalItems, from);
        }
        
        public static Pagination<TK> AutoMap<T, TK>(this Pagination<T> from, IMapper mapper)
        {
            return new Pagination<TK>(from.Items.Select(x=> mapper.Map<TK>(x)), from.TotalItems, from);
        }
    }
}
