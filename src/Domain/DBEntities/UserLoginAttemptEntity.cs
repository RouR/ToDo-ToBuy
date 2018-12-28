using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Interfaces;

namespace Domain.DBEntities
{
    [Table("LoginAttempt")]
    public class UserLoginAttemptEntity : IPublicIdEntity
    {
        [Key]
        public Guid PublicId { get; set; }

        [Required]
        [MaxLength(80)]
        public string Email { get; set; }

        [MaxLength(Constants.IpStringMaxLength)]
        public string IP { get; set; }

        public DateTime AtUtc { get; set; }
    }
}