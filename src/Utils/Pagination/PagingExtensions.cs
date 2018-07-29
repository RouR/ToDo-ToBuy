using System.Linq;

namespace Utils.Pagination
{
    public static class PagingExtensions
    {
        public const int DefaultPageSize = 25;

        public static Pagination<T> AsPagination<T>(this IQueryable<T> query, int pageNumber)
        {
            return AsPagination<T>(query, DefaultPageSize);
        }

        public static Pagination<T> AsPagination<T>(this IQueryable<T> query, IPaginationSetting settings)
        {
            var results = query.Skip((settings.Page - 1) * settings.PageSize).Take(settings.PageSize);
            return new Pagination<T>(results, settings.Page, settings);
        }
    }
}
