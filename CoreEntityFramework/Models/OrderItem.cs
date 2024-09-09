namespace CoreEntityFramework.Models
{
    public class OrderItem
    {

        public int OrderItemId { get; set; }

        //Unit Price at time of purchase
        public decimal OrderItemUnitPrice { get; set; }

        //The number of items ordered
        public decimal OrderItemQty { get; set; }

        
        //An order item belongs to one order
        public required virtual Order Order { get; set; }
        public int OrderId { get; set; }

        //An order item belongs to one product
        public required virtual Product Product { get; set; }
        public int ProductId { get; set; }

        
    }
}
