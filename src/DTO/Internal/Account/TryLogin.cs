using System.ComponentModel.DataAnnotations;
using Domain;
using Domain.DBEntities;
using Domain.Interfaces;
using DTO.Public.Account;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Internal.Account
{
    [ServiceName(nameof(Service.Account))]
    public class TryLoginRequest: IProduce<TryLoginResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        
        [MaxLength(Constants.IpStringMaxLength)]
        public string IP { get; set; }
    }

    [ServiceName(nameof(Service.Account))]
    public class TryLoginResponse: IErrorable<UserEntity>, IProduce<LoginResponse>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public UserEntity Data { get; set; }
    }
}