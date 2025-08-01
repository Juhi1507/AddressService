namespace OrderService.Models
{
    public class AdminAnalytics
    {
        public int TotalOrders { get; set; }
        public decimal TotalSales { get; set; }
        public int PendingOrders { get; set; }
        public int ShippedOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int TotalCustomers { get; set; }
        public List<SalesChartPointDto> SalesChart { get; set; } = new();
    }
}
