using CoreEntityFramework.Interfaces;
using CoreEntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreEntityFramework.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        // dependency injection of database context
        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto?> FindOrder(int id)
        {
            //include the (o)rder customer
            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            // no order item found
            if (order == null)
            {
                return null;
            }
            // create an instance of orderItemDto
            OrderDto orderDto = new OrderDto()
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate.ToString("yyyy-MM-dd"),
                CustomerName = order.Customer.CustomerName
            };
            return orderDto;

        }

    }
}
