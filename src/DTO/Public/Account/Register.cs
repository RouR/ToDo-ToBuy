using System;
using DTO.Internal.Account;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Public.Account
{
    [ServiceName(nameof(Service.Account))]
    public class RegisterRequest : IProduce<CreateUserRequest>
    {
        
    }
    [ServiceName(nameof(Service.Account))]
    public class RegisterResponse: IErrorable<Guid>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public Guid Data { get; set; }
    }
}