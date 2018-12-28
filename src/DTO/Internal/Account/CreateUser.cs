using System;
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
    public class CreateUserRequest: IProduce<CreateUserResponse>, IPublicIdEntity
    {
        [Required]
        public virtual Guid PublicId { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(10)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(80)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Empty passwords is not allowed")]
        [MinLength(8)]
        [MaxLength(80)]
        public string Password { get; set; }
        
        
        [MaxLength(Constants.IpStringMaxLength)]
        public virtual string IP { get; set; }
    }

    [ServiceName(nameof(Service.Account))]
    public class CreateUserResponse: IErrorable<UserEntity>, IProduce<RegisterResponse>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public UserEntity Data { get; set; }
    }
}