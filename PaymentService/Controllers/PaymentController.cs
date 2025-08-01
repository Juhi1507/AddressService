using Microsoft.AspNetCore.Mvc;
using PaymentService.Models;
using PaymentService.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] Payment payment)
        {
            if (payment == null) return BadRequest("Invalid payment data.");

            var processedPayment = await _paymentRepository.ProcessPayment(payment);
            return Ok(processedPayment);
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentByOrderId(int orderId)
        {
            var payment = await _paymentRepository.GetPaymentByOrderId(orderId);
            if (payment == null) return NotFound("Payment not found for this order.");

            return Ok(payment);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetPaymentsByCustomerId(int customerId)
        {
            var payments = await _paymentRepository.GetPaymentsByCustomerId(customerId);
            return Ok(payments);
        }

        [HttpPut("update-status/{paymentId}")]
        public async Task<IActionResult> UpdatePaymentStatus(int paymentId, [FromBody] string status)
        {
            bool updated = await _paymentRepository.UpdatePaymentStatus(paymentId, status);
            if (!updated) return NotFound("Payment not found.");

            return Ok("Payment status updated successfully.");
        }
    }
}
