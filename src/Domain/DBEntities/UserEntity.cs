using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DBEntities
{
    [Table("Users")]
    public class UserEntity : DBEntity
    {    
        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required]
        [MaxLength(80)]
        public string Email { get; set; }

        [Required]
        [MaxLength(SaltMaxLength)]
        public string Salt { get; set; }

        [Required]
        [MaxLength(SaltedPwdMaxLength)]
        public string SaltedPwd { get; set; }

        public const int SaltMaxLength = 80;
        public const int SaltedPwdMaxLength = 80;

        [MaxLength(Constants.IpStringMaxLength)]
        public string IP { get; set; }
    }
}