using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using OrderService.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order == null) return BadRequest("Invalid order data.");

            var createdOrder = await _orderRepository.CreateOrder(order);
            return Ok(createdOrder);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomerId(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerId(customerId);
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null) return NotFound("Order not found.");

            return Ok(order);
        }
        [HttpPut("update-status/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] StatusUpdateRequest request)
        {
            bool updated = await _orderRepository.UpdateOrderStatus(orderId, request.Status);
            if (!updated) return NotFound("Order not found.");

            return Ok("Order status updated successfully.");
        }



        [HttpDelete("delete/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            bool deleted = await _orderRepository.DeleteOrder(orderId);
            if (!deleted) return NotFound("Order not found.");

            return Ok("Order deleted successfully.");
        }
        [HttpPut("update-estimated-date/{orderId}")]
        public async Task<IActionResult> UpdateEstimatedDeliveryDate(int orderId, [FromBody] EstimatedDateRequest request)
        {
            bool updated = await _orderRepository.UpdateEstimatedDeliveryDate(orderId, request.EstimatedDeliveryDate);
            if (!updated) return NotFound("Order not found.");

            return Ok("Estimated delivery date updated.");
        }

        [HttpGet("analytics")]
        public async Task<ActionResult<object>> GetAdminAnalytics()
        {
            var result = await _orderRepository.GetAdminAnalyticsAsync();
            return Ok(result);
        }


        public class EstimatedDateRequest
        {
            public DateTime EstimatedDeliveryDate { get; set; }
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            return Ok(orders);
        }

        public class StatusUpdateRequest
        {
            public string Status { get; set; }
        }


    }
}
