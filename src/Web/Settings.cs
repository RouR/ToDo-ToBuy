using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shared;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class Settings
    {
        //public static string Header { get; set; } = "HeaderRequestId";

        #region Jwt

        public static readonly SecurityKey JwtSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("someSecretKey@6s4d!@#8o7anselm8*jfk"));

        public static readonly TimeSpan JwtExpires = TimeSpan.FromDays(10);

        // we can force client logout by change this
        public const string JwtIssuer = "tbtd-app";
        public const string JwtAudience = "app-api";

        #endregion
    }
}