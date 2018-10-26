using Domain.DBEnities;
using DTO.Public.Account;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Internal.Account
{
    public class CreateUserRequest: IProduce<CreateUserResponse>
    {
        
    }
    
    public class CreateUserResponse: IErrorable<UserEntity>, IProduce<RegisterResponse>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public UserEntity Data { get; set; }
    }
}