using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DBEnities
{
    [Table("Users")]
    public class UserEntity : DBEntity
    {
        [MaxLength(80)]
        public string Email { get; set; }
    }
}