using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Web.Utils
{
    public static class AuthHelper
    {
        /// <summary>
        /// use HttpContext.GetUserId()
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Guid GetUserId(this HttpContext context)
        {
            var id = context.User.Identity.IsAuthenticated
                ? context.User.Claims.SingleOrDefault(x=> x.Type == Settings.JwtUserIdClaimName)?.Value
                : string.Empty;
            return Guid.Parse(id);
        }
    }
}