using CustomerService.Data;
using CustomerService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> RegisterCustomer(Customer customer)
        {
            customer.PasswordHash = HashPassword(customer.PasswordHash);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerById(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return null;

            return new Customer
            {
                CustomerID = customer.CustomerID,
                FullName = customer.FullName,
                Email = customer.Email,
                MobileNumber = customer.MobileNumber,
                DateOfBirth = customer.DateOfBirth,
                Gender = customer.Gender,
                GitHubId = customer.GitHubId
            };
        }


        public async Task<Customer> GetCustomerByEmailAndPassword(string email, string password)
        {
            string hashedPassword = HashPassword(password);
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email && c.PasswordHash == hashedPassword);
        }

        public async Task<Customer> GetCustomerByGitHubId(string gitHubId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.GitHubId == gitHubId);
        }

        public async Task<bool> UpdateCustomer(Customer customer)
        {
            customer.PasswordHash = HashPassword(customer.PasswordHash);
            _context.Customers.Update(customer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCustomer(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return false;

            _context.Customers.Remove(customer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> CountCustmomer()
        {
            var count = await _context.Customers.CountAsync();
            return count;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

    }
}
