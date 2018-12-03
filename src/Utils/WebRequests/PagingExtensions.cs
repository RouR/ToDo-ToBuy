using System.Linq;
using Domain.Interfaces;

namespace Utils.WebRequests
{
    public static class PagingExtensions
    {
        private class DefaultPaginationSetting : IPaginationSetting
        {
            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 25;
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
    }
}
