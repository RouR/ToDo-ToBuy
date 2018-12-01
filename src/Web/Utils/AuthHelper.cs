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
        public static string GetUserId(this HttpContext context)
        {
            return context.User.Identity.IsAuthenticated
                ? context.User.Claims.SingleOrDefault(x=> x.Type == Settings.JwtUserIdClaimName)?.Value
                : string.Empty;
        }
    }
}