using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrderService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;
        private readonly HttpClient _httpClient;

        public OrderRepository(OrderDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<Order> CreateOrder(Order order)
        {
            if (order.OrderItems == null || !order.OrderItems.Any())
            {
                throw new ArgumentException("Order must contain at least one item.");
            }

            // Calculate total amount from OrderItems
            order.TotalAmount = order.OrderItems.Sum(item => item.Price * item.Quantity);

            order.OrderDate = DateTime.UtcNow;
            order.Status = "Pending"; // Default status

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }


        public async Task<List<Order>> GetOrdersByCustomerId(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerID == customerId)
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderID == orderId);
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            _context.Orders.Remove(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateEstimatedDeliveryDate(int orderId, DateTime date)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.EstimatedDeliveryDate = date;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        // public async Task<ActionResult<AdminAnalytics>> GetAdminAnalyticsAsync()
        // {
        //     var orders = await _context.Orders.ToListAsync();

        //     var totalOrders = orders.Count;
        //     var totalSales = orders.Sum(o => o.TotalAmount);
        //     var pendingOrders = orders.Count(o => o.Status == "Pending");
        //     var shippedOrders = orders.Count(o => o.Status == "Shipped");
        //     var deliveredOrders = orders.Count(o => o.Status == "Delivered");

        //     int totalCustomers = 0;

        //     var response = await _httpClient.GetAsync("https://localhost:7112/api/Customer/count");

        //     if (response.IsSuccessStatusCode)
        //     {
        //         var content = await response.Content.ReadAsStringAsync();
        //         totalCustomers = int.Parse(content);
        //     }

        //     var salesChart = orders
        //   .GroupBy(o => o.OrderDate.Date)
        //.Select(group => new SalesChartPointDto
        //{
        //    Date = group.Key.ToString("yyyy-MM-dd"),
        //    TotalSales = group.Sum(o => o.TotalAmount)
        //})
        //.OrderBy(point => point.Date)
        //.ToList();
        //     return new AdminAnalytics
        //     {
        //         TotalOrders = totalOrders,
        //         TotalSales = totalSales,
        //         PendingOrders = pendingOrders,
        //         ShippedOrders = shippedOrders,
        //         DeliveredOrders = deliveredOrders,
        //         TotalCustomers = totalCustomers,
        //         SalesChart = salesChart
        //     };

        // }
        public async Task<ActionResult<object>> GetAdminAnalyticsAsync()
        {
            var orders = await _context.Orders.ToListAsync();

            var totalOrders = orders.Count;
            var totalSales = orders.Sum(o => o.TotalAmount);
            var pendingOrders = orders.Count(o => o.Status == "Pending");
            var shippedOrders = orders.Count(o => o.Status == "Shipped");
            var deliveredOrders = orders.Count(o => o.Status == "Delivered");

            int totalCustomers = 0;
            var response = await _httpClient.GetAsync("https://localhost:7112/api/Customer/count");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                totalCustomers = int.Parse(content);
            }

            var salesChart = orders
                .GroupBy(o => o.OrderDate.Date)
                .Select(group => new SalesChartPointDto
                {
                    Date = group.Key.ToString("yyyy-MM-dd"),
                    TotalSales = group.Sum(o => o.TotalAmount)
                })
                .OrderBy(point => point.Date)
                .ToList();

            return new AdminAnalytics
            {
                TotalOrders = totalOrders,
                TotalSales = totalSales,
                PendingOrders = pendingOrders,
                ShippedOrders = shippedOrders,
                DeliveredOrders = deliveredOrders,
                TotalCustomers = totalCustomers,
                SalesChart = salesChart
            };



        }



    }
}
