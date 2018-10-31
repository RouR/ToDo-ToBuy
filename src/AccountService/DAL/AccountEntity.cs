using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountService.DAL
{
    [Table("Accounts")]
    public class AccountEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}