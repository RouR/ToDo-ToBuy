using System;
using Domain.DBEnities;
using Domain.Interfaces;
using DTO.Public.Account;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Internal.Account
{
    [ServiceName(nameof(Service.Account))]
    public class FindUserRequest: IProduce<FindUserResponse>, IPublicIdEntity
    {
        public Guid PublicId { get; set; }
    }
    [ServiceName(nameof(Service.Account))]
    public class FindUserResponse: IErrorable<UserEntity>, IProduce<LoginResponse>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public UserEntity Data { get; set; }
    }
}