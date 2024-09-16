using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreEntityFramework;
using CoreEntityFramework.Models;

namespace CoreEntityFramework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        // dependency injection of database context
        public OrderItemsController(AppDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Returns a list of Ordered Items, each represented by an OrderItemDto with their associated Product, Order, and Customer
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{OrderItemDto},{OrderItemDto},..]
        /// </returns>
        /// <example>
        /// GET: api/OrderItems/List -> [{OrderItemDto},{OrderItemDto},..]
        /// </example>
        [HttpGet(template:"List")]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> ListOrderItems()
        {
            // include will join the order(i)tem with 1 product, 1 order, 1 customer
            List<OrderItem> orderItems = await _context.OrderItems
                .Include(i => i.Product)
                .Include(i => i.Order)
                .Include(i => i.Order.Customer)
                .ToListAsync();
            // empty list of data transfer object OrderItemDto
            List<OrderItemDto> orderItemDtos = new List<OrderItemDto>();
            // foreach Order Item record in database
            foreach (OrderItem orderItem in orderItems)
            {
                // create new instance of OrderItemDto, add to list
                orderItemDtos.Add(new OrderItemDto(){ 
                        OrderItemId = orderItem.OrderItemId,
                        OrderItemUnitPrice = orderItem.OrderItemUnitPrice,
                        OrderItemQty = orderItem.OrderItemQty,
                        OrderItemSubtotal = orderItem.OrderItemQty * orderItem.OrderItemUnitPrice,
                        ProductId = orderItem.ProductId,
                        ProductSKU = orderItem.Product.ProductSKU,
                        OrderId = orderItem.OrderId,
                        OrderDate = orderItem.Order.OrderDate.ToString("yyyy-MM-dd"),
                        CustomerName = orderItem.Order.Customer.CustomerName
                    });
            }
            // return 200 OK with OrderItemDtos
            return Ok(orderItemDtos);
        }

        /// <summary>
        /// Returns a single Ordered Item specified by its {id}, represented by an Order Item Dto with its associated Product, Order, and Customer
        /// </summary>
        /// <param name="id">The ordered item id</param>
        /// <returns>
        /// 200 OK
        /// {OrderItemDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/OrderItems/Find/1 -> {OrderItemDto}
        /// </example>
        [HttpGet(template:"Find/{id}")]
        public async Task<ActionResult<OrderItemDto>> FindOrderItem(int id)
        {
            // include will join order(i)tem with 1 product, 1 order, 1 customer
            // first or default async will get the first order(i)tem matching the {id}
            var orderItem = await _context.OrderItems
                .Include(i => i.Product)
                .Include(i => i.Order)
                .Include(i => i.Order.Customer)
                .FirstOrDefaultAsync(i => i.OrderItemId == id);

            // if the item could not be located, return 404 Not Found
            if (orderItem == null)
            {
                return NotFound();
            }
            // create an instance of orderItemDto
            OrderItemDto orderItemDto = new OrderItemDto()
            {
                OrderItemId = orderItem.OrderItemId,
                OrderItemUnitPrice = orderItem.OrderItemUnitPrice,
                OrderItemQty = orderItem.OrderItemQty,
                OrderItemSubtotal = orderItem.OrderItemQty * orderItem.OrderItemUnitPrice,
                ProductId = orderItem.ProductId,
                ProductSKU = orderItem.Product.ProductSKU,
                OrderId = orderItem.OrderId,
                OrderDate = orderItem.Order.OrderDate.ToString("yyyy-MM-dd"),
                CustomerName = orderItem.Order.Customer.CustomerName
            };
            //return 200 OK with orderItemDto
            return Ok(orderItemDto);
        }

        /// <summary>
        /// Updates an Ordered Item
        /// </summary>
        /// <param name="id">The ID of Order Item to update</param>
        /// <param name="orderItemDto">The required information to update the ordered item (OrderItemId, OrderItemUnitPrice,OrderItemQty,ProductId,OrderId)</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        [HttpPut(template:"Update/{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, OrderItemDto orderItemDto)
        {
            // {id} in URL must match OrderItemId in POST Body
            if (id != orderItemDto.OrderItemId)
            {
                //400 Bad Request
                return BadRequest();
            }
            // attempt to find associated Product and Order in DB by looking up ProductId and OrderId foreign key
            var product = await _context.Products.FindAsync(orderItemDto.ProductId);
            var order = await _context.Orders.FindAsync(orderItemDto.OrderId);
            // Posted data must link to valid entity
            if (product == null || order == null)
            {
                //404 Not Found
                return NotFound();
            }
            // Create instance of OrderItem
            OrderItem orderItem = new OrderItem()
            {
                OrderItemId = Convert.ToInt32(orderItemDto.OrderItemId),
                OrderItemUnitPrice = orderItemDto.OrderItemUnitPrice,
                OrderItemQty = orderItemDto.OrderItemQty,
                ProductId = orderItemDto.ProductId,
                Product = product,
                Order = order,
                OrderId = orderItemDto.OrderId
            };
            // flags that the object has changed
            _context.Entry(orderItem).State = EntityState.Modified;

            try
            {
                // SQL Equivalent: Update Orderitems set ... where OrderItemId={id}
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Adds an Order Item
        /// </summary>
        /// <param name="orderItemDto">The required information to add the ordered item (OrderItemUnitPrice,OrderItemQty,ProductId,OrderId)</param>
        /// <returns>
        /// 201 Created
        /// Location: api/OrderItems/Find/{OrderItemId}
        /// {OrderItemDto}
        /// or
        /// 404 Not Found
        /// </returns>
        [HttpPost(template:"Add")]
        public async Task<ActionResult<OrderItem>> AddOrderItem(OrderItemDto orderItemDto)
        {
            // attempt to find associated Product and Order in DB by looking up ProductId and OrderId foreign key
            var product = await _context.Products.FindAsync(orderItemDto.ProductId);
            var order = await _context.Orders.FindAsync(orderItemDto.OrderId);
            // Posted data must link to valid entity
            if (product == null || order == null)
            {
                //404 Not Found
                return NotFound();
            }

            OrderItem orderItem = new OrderItem(){
                OrderItemUnitPrice = orderItemDto.OrderItemUnitPrice,
                OrderItemQty = orderItemDto.OrderItemQty,
                ProductId = orderItemDto.ProductId,
                Product = product,
                Order = order,
                OrderId = orderItemDto.OrderId
            };
            // SQL Equivalent: Insert into orderitems (..) values (..)
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            // returns 201 Created with Location
            return CreatedAtAction("GetOrderItem", new { id = orderItem.OrderItemId }, orderItemDto);
        }

        /// <summary>
        /// Deletes the Ordered Item
        /// </summary>
        /// <param name="id">The id of the Order Item to delete</param>
        /// <returns>
        /// 201 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            // Order Item must exist in the first place
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //TODO:
        
        //ListOrderItemsForOrder
        
        //ListOrderItemsForProduct

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.OrderItemId == id);
        }
    }
}
