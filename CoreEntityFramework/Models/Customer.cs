using System.ComponentModel.DataAnnotations;

namespace CoreEntityFramework.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        public required string CustomerName { get; set; }

        public required string CustomerEmail { get; set; }

        //A customer can have many orders
        public ICollection<Order>? Orders { get; set; }
    }
}
