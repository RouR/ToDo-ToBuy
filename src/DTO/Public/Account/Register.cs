using System;
using System.Collections.Generic;
using Domain.Interfaces;
using DTO.Internal.Account;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;
using Newtonsoft.Json;

namespace DTO.Public.Account
{
    [ServiceName(nameof(Service.Web))]
    public class RegisterRequest : CreateUserRequest, IProduce<CreateUserRequest>
    {
        [JsonIgnore]
        public override Guid PublicId { get; set; }

        [JsonIgnore]
        public override string IP { get; set; }
    }

    [ServiceName(nameof(Service.Web))]
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