using System.Linq;
using Domain.Interfaces;

namespace Utils.WebRequests
{
    public static class IFilterableHelper
    {
        public static IQueryable<K> Filter<T, K>(this IFilterable<T, K> obj, IQueryable<K> data) where T : IFilter<K>
        {
            return ((IFilter<K>)obj.Filter).ApplyFiler(data);
        }
    }
}