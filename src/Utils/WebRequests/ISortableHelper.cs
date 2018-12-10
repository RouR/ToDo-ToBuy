using System.Linq;
using System.Reflection;
using Domain.Interfaces;

namespace Utils.WebRequests
{
    public static class ISortableHelper
    {
        public static IQueryable<K> Sort<K>(this ISortable obj, IQueryable<K> data)
        {
            if (!string.IsNullOrEmpty(obj.OrderBy))
            {
                var propertyInfo = typeof(K).GetProperty(obj.OrderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null) //field is not exist
                    return data;
                
                if(obj.Asc)
                    data = data.OrderBy(x => propertyInfo.GetValue(x, null));
                else
                    data = data.OrderByDescending(x => propertyInfo.GetValue(x, null));
            }
            return data;
        }
    }
}