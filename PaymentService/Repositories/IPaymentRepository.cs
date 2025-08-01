using PaymentService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentService.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> ProcessPayment(Payment payment);
        Task<Payment> GetPaymentByOrderId(int orderId);
        Task<List<Payment>> GetPaymentsByCustomerId(int customerId);
        Task<bool> UpdatePaymentStatus(int paymentId, string status);
    }
}
