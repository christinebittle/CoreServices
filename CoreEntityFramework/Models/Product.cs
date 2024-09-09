using System.ComponentModel.DataAnnotations;

namespace CoreEntityFramework.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        
        public required string ProductName { get; set; }

        public required string ProductSKU { get; set; }

        //A product can have many categories
        public ICollection<Category>? Categories { get; set; }

        //A product can be a part of many ordered items
        public ICollection<OrderItem>? Items { get; set; }

    }
}
