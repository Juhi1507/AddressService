using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> ProcessPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> GetPaymentByOrderId(int orderId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.OrderID == orderId);
        }

        public async Task<List<Payment>> GetPaymentsByCustomerId(int customerId)
        {
            return await _context.Payments.Where(p => p.CustomerID == customerId).ToListAsync();
        }

        public async Task<bool> UpdatePaymentStatus(int paymentId, string status)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null) return false;

            payment.Status = status;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
