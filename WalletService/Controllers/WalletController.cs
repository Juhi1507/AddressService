using WalletService.Models;
using WalletService.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Stripe;

namespace WalletService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IConfiguration _configuration;

        public WalletController(IWalletRepository walletRepository, IConfiguration configuration)
        {
            _walletRepository = walletRepository;
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        [HttpPost("create/{customerId}")]
        public async Task<IActionResult> CreateWallet(int customerId)
        {
            var wallet = await _walletRepository.CreateWallet(customerId);
            return Ok(wallet);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetWalletByCustomerId(int customerId)
        {
            var wallet = await _walletRepository.GetWalletByCustomerId(customerId);
            if (wallet == null) return NotFound("Wallet not found.");
            return Ok(wallet);
        }

        //[HttpPost("add-funds")]
        //public async Task<IActionResult> AddFunds(int customerId, decimal amount)
        //{
        //    bool success = await _walletRepository.AddFunds(customerId, amount);
        //    if (!success) return BadRequest("Failed to add funds.");
        //    return Ok("Funds added successfully.");
        //}

        [HttpPost("add-funds")]
        public async Task<IActionResult> AddFunds([FromBody] PaymentRequest request)
        {
            if (request.Amount <= 0)
                return BadRequest("Invalid amount.");

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(request.Amount * 100),
                Currency = "inr",
                Description = "Wallet top-up for shopping",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions { Enabled = true }
            };

            var service = new PaymentIntentService();
            var intent = service.Create(options);

            return Ok(new { clientSecret = intent.ClientSecret });
        }


        [HttpPost("confirm-payment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequest request)
        {
            var success = await _walletRepository.ConfirmPayment(request.CustomerId, request.Amount);
            if (!success) return BadRequest("Failed to add funds to wallet.");

            return Ok(new { message = "Wallet updated successfully." });
        }

        [HttpPost("deduct-funds")]
        public async Task<IActionResult> DeductFunds([FromBody] DeductFundsRequest request)
        {
            if (request.CustomerId <= 0 || request.Amount <= 0)
                return BadRequest("Invalid request data.");

            bool success = await _walletRepository.DeductFunds(request.CustomerId, request.Amount);
            if (!success) return BadRequest("Insufficient balance or wallet not found.");

            return Ok(new { message = "Amount deducted successfully" });
        }

    }

    public class PaymentRequest
    {
        public decimal Amount { get; set; }
    }

    public class ConfirmPaymentRequest
    {
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
    }

    public class DeductFundsRequest
    {
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
    }



}
