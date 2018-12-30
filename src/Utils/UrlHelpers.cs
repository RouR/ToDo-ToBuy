using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class UrlHelpers
    {
        public static Dictionary<string, string> ToFlatDictionary(this object request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var ret = new Dictionary<string, string>();

            foreach (var propertyInfo in request.GetType().GetProperties()
                .Where(x => x.CanRead)
                .Where(x => x.GetValue(request, null) != null))
            {
                if(propertyInfo.PropertyType.IsClass
                    && !propertyInfo.PropertyType.IsPrimitive
                    && !propertyInfo.PropertyType.Name.Equals("String", StringComparison.InvariantCultureIgnoreCase)) 
                {
                    var prefix = propertyInfo.Name + ".";
                    var subProperties = ToFlatDictionary(propertyInfo.GetValue(request, null));
                    foreach (var pair in subProperties)
                    {
                        ret.Add(prefix + pair.Key, pair.Value);
                    }
                }else
                    ret.Add(propertyInfo.Name, propertyInfo.GetValue(request, null)?.ToString());
            }

            return ret;
        }
    }
}