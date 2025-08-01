using CustomerService.Models;
using CustomerService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Customer customer)
        {
            if (customer == null)
                return BadRequest("Invalid customer data.");

            if (customer.Email == "admin@example.com")
            {
                customer.Role = "Admin";
            }
            else
            {
                customer.Role = "User";
            }

            var registeredCustomer = await _customerRepository.RegisterCustomer(customer);
            return Ok(registeredCustomer);
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetAllCustomers()
        {
            return Ok(await _customerRepository.GetAllCustomers());
        }

        [HttpGet("GetById/{customerId}")]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {
            var customer = await _customerRepository.GetCustomerById(customerId);
            if (customer == null)
            {
                return NotFound(new { message = "Customer not found" });
            }
            return Ok(customer);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var customer = await _customerRepository.GetCustomerByEmailAndPassword(loginRequest.Email, loginRequest.Password);
            if (customer == null) return Unauthorized("Invalid credentials.");
            return Ok(new
            {
                CustomerID = customer.CustomerID,
                FullName = customer.FullName,
                Email = customer.Email,
                Role = customer.Role
            });
        }

        [HttpPost("github-login")]
        public async Task<IActionResult> GitHubLogin([FromBody] GitHubLoginRequest request)
        {
            var customer = await _customerRepository.GetCustomerByGitHubId(request.GitHubId);
            if (customer == null)
            {
                customer = new Customer
                {

                    GitHubId = request.GitHubId,
                    GitHubToken = request.GitHubToken
                };
                await _customerRepository.RegisterCustomer(customer);
            }
            return Ok(customer);
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomer(int customerId, [FromBody] Customer customer)
        {
            if (customer.CustomerID != customerId)
                return BadRequest("Customer ID mismatch.");

            bool updated = await _customerRepository.UpdateCustomer(customer);
            if (!updated) return NotFound("Customer not found.");

            return Ok("Customer updated successfully.");
        }

        [HttpDelete("DeleteCustomer/{customerId}")]
        public async Task<IActionResult> DeleteCustomer(int customerId)
        {
            bool deleted = await _customerRepository.DeleteCustomer(customerId);
            if (!deleted) return NotFound("Customer not found.");

            return Ok("Customer deleted successfully.");
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCustomerCount()
        {
            var count = await _customerRepository.CountCustmomer();
            return Ok(count);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class GitHubLoginRequest
    {
        public string GitHubId { get; set; }
        public string GitHubToken { get; set; }
        //public string FullName { get; set; }
        //public string Email { get; set; }
    }
}
