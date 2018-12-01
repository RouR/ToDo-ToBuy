using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;
using DTO.Internal.Account;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Public.Account
{
    [ServiceName("Web")]
    public class RegisterRequest : IProduce<CreateUserRequest>
    {
        [Required]
        [MinLength(4)]
        [MaxLength(100)]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Empty passwords is not allowed")]
        [MinLength(8)]
        [MaxLength(100)]
        public string Password { get; set; }
    }
    [ServiceName("Web")]
    public class RegisterResponse: IErrorable<Guid>, IServerValidation
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public Guid Data { get; set; }

        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }
    }
}