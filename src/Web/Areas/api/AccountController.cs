using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using AutoMapper;
using DTO.Internal.Account;
using DTO.Public.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared;
using Utils;
using Utils.WebRequests;

namespace Web.Areas.api
{
    [ApiVersion("0.1")]
    [GlobalValidator]
    [Area("api")]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly AccountServiceClient _accountServiceClient;
        private readonly IIpAddressParser _ipAddressParser;
        private readonly IMapper _mapper;

        public AccountController(AccountServiceClient accountServiceClient, IIpAddressParser ipAddressParser, IMapper mapper)
        {
            _accountServiceClient = accountServiceClient;
            _ipAddressParser = ipAddressParser;
            _mapper = mapper;
        }

        /// <summary>
        /// try login, get Jwt token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<LoginResponse> Login([FromBody]LoginRequest request)
        {
            if (request == null)
            {
                var response = new LoginResponse();
                response.SetError("Invalid login request");
                return response;
            }

            var res = await _accountServiceClient.User_TryLogin(new TryLoginRequest()
            {
                UserName = request.UserName,
                Password = request.Password,
                IP = _ipAddressParser.GetClientIp(HttpContext).ToString()
            });

            if (res.HasError)
            {
                var response = new LoginResponse();
                //response.SetError("Invalid login/password");
                response.SetError(res.Message);

                HttpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                return response;
            }

            if (res.Data == null)
            {
                var response = new LoginResponse();
                response.SetError("Internal error (TLR)");
                return response;
            }

            var signinCredentials = new SigningCredentials(Settings.JwtSigningKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: Settings.JwtIssuer,
                audience: Settings.JwtAudience,
                claims: new List<Claim>()
                {
                    // ClaimTypes.Name serialized as "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
                    // and it is too long
                    // new Claim(ClaimTypes.Name, "name-test"),
                    new Claim(Settings.JwtUserNameClaimName, res.Data.Name),
                    new Claim(Settings.JwtUserIdClaimName, res.Data.PublicId.ToString()),
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

        /// <summary>
        /// register new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<RegisterResponse> Register([FromBody]RegisterRequest request)
        {
            RegisterResponse response;
            
            if (request == null)
            {
                response = new RegisterResponse();
                response.SetError("Invalid register request");
                return response;
            }

            request.PublicId = Guid.NewGuid();
            request.IP = _ipAddressParser.GetClientIp(HttpContext).ToString();
            var registerResponse = await _accountServiceClient.User_Register(_mapper.Map<CreateUserRequest>(request));

            response = new RegisterResponse()
            {
                HasError = registerResponse.HasError,
                Message = registerResponse.Message,
                Data = registerResponse.Data?.PublicId ?? Guid.Empty
            };

            return response;
        }
    }
}