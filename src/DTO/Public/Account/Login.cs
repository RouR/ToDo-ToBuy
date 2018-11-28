using Domain.Interfaces;
using DTO.Internal.Account;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Public.Account
{
    [ServiceName("Web")]
    public class LoginRequest : IProduce<FindUserRequest>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    [ServiceName("Web")]
    public class LoginResponse: IErrorable<string>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        /// <summary>
        /// Jwt token
        /// </summary>
        public string Data { get; set; }
    }
  
}