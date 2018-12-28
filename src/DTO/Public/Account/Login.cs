using Domain.Interfaces;
using DTO.Internal.Account;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Public.Account
{
    [ServiceName(nameof(Service.Account))]
    public class LoginRequest : IProduce<TryLoginRequest>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    [ServiceName(nameof(Service.Account))]
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