using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using DTO.Public.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Utils.WebRequests;
using Web.Utils;

namespace Web.Areas.api
{
    [GlobalValidator]
    [Area("api")]
    [ApiVersion("0.1")]
    [Route("api/[controller]/[action]")]
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
                        // ClaimTypes.Name serialized as "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
                        // and it is too long
                        // new Claim(ClaimTypes.Name, "name-test"),
                        new Claim(Settings.JwtUserIdClaimName, "name-test"),
                        
                        //new Claim("claim-test", "someData"),
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
        
        /// <summary>
        /// register new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public RegisterResponse Register([FromBody]RegisterRequest request)
        {
            RegisterResponse response;

            
            
            if (request == null)
            {
                response = new RegisterResponse();
                response.SetError("Invalid register request");
                return response;
            }
            
            response = new RegisterResponse();
            response.SetError("Register internal error");
            return response;
        }
    }
}