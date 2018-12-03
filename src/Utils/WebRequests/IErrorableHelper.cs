using Domain.Interfaces;

namespace Utils.WebRequests
{
    public static class IErrorableHelper
    {
        public static IErrorable<T> SetError<T>(this IErrorable<T> obj, string message)
        {
            if (obj == null)
                return null;
            
            obj.HasError = true;
            obj.Message = message;
            return obj;
        }
    }
}