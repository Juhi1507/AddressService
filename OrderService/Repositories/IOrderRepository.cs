using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrder(Order order);
        Task<List<Order>> GetAllOrders();

        Task<List<Order>> GetOrdersByCustomerId(int customerId);
        Task<Order> GetOrderById(int orderId);
        Task<bool> UpdateOrderStatus(int orderId, string status);
        Task<bool> DeleteOrder(int orderId);
        Task<ActionResult<object>> GetAdminAnalyticsAsync();
        Task<bool> UpdateEstimatedDeliveryDate(int orderId, DateTime date);
    }
}
