using Domain.Enums;

namespace Domain.Models
{
    public class Price
    {
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
    }
}