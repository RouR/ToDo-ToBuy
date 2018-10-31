using System;
using Domain.DBEnities;
using Domain.Interfaces;
using DTO.Public.Account;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Internal.Account
{
    [ServiceName(nameof(Service.Account))]
    public class CreateUserRequest: IProduce<CreateUserResponse>, IPublicIdEntity
    {
        public Guid PublicId { get; set; }
    }
    [ServiceName(nameof(Service.Account))]
    public class CreateUserResponse: IErrorable<UserEntity>, IProduce<RegisterResponse>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public UserEntity Data { get; set; }
    }
}