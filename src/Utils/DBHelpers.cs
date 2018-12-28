using System;
using System.Transactions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Utils
{
    public static class DBHelpers
    {
        public static T TryDoTransaction<T, K>(DbContext dbContext, Func<(bool, T)> action) where T: IErrorable<K>
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var (save, result) = action();

                    if (save)
                    {
                        dbContext.SaveChanges();
                        scope.Complete();
                    }
                    return result;
                }
            }
            catch (Exception e)
            {
                var instance = (T) Activator.CreateInstance(typeof(T));
                instance.HasError = true;
                instance.Message = e.Message;
                return instance;
            }
        }
    }
}
