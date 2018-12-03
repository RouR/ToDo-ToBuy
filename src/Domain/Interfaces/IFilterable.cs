using System.Linq;

namespace Domain.Interfaces
{
    public interface IFilter<T>
    {
        IQueryable<T> ApplyFiler(IQueryable<T> data);
    }

    public interface IFilterable<T, K> where T : IFilter<K>
    {
        T Filter { get; set; }
    }
}