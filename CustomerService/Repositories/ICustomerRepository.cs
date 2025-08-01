using CustomerService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerService.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> RegisterCustomer(Customer customer);
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerByEmailAndPassword(string email, string password);
        Task<Customer> GetCustomerByGitHubId(string gitHubId);
        Task<bool> UpdateCustomer(Customer customer);
        Task<bool> DeleteCustomer(int customerId);
        Task<Customer> GetCustomerById(int customerId);
        Task<int> CountCustmomer();

    }
}
