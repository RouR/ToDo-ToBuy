using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using DTO.Public.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Utils;

namespace Web.Areas.api
{
    [Area("api")]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        /// <summary>
        /// try login, get Jwt token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public LoginResponse Login([FromBody]LoginRequest request)
        {
            if (request == null)
            {
                var response = new LoginResponse();
                response.SetError("Invalid login request");
                return response;
            }
            
            if (request.UserName == "test" && request.Password == "123123")
            {
                var signinCredentials = new SigningCredentials(Settings.JwtSigningKey, SecurityAlgorithms.HmacSha256);
 
                var tokeOptions = new JwtSecurityToken(
                    issuer: Settings.JwtIssuer,
                    audience: Settings.JwtAudience,
                    claims: new List<Claim>()
                    {
                        new Claim("name", "name-test"),
                        new Claim("claim-test", "someData"),
                    },
                    expires: DateTime.Now.Add(Settings.JwtExpires),
                    signingCredentials: signinCredentials
                );
 
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return new LoginResponse()
                {
                    Data = tokenString
                };
            }
            else
            {
                var response = new LoginResponse();
                response.SetError("Invalid login/password");
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return response;
            }
        }
    }
}