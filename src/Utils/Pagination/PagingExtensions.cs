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

        public static Pagination<T> AsPagination<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            int count = query.Count();

            var results = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return new Pagination<T>(results, pageNumber, pageSize, count);
        }
    }
}
