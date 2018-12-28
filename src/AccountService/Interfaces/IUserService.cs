using DTO.Internal.Account;

namespace AccountService.Interfaces
{
    public interface IUserService
    {
        CreateUserResponse Register(CreateUserRequest model);
        TryLoginResponse Login(TryLoginRequest model);
    }
}