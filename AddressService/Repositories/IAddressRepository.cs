using AddressService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AddressService.Repositories
{
    public interface IAddressRepository
    {
        Task<Address> AddAddress(Address address);
        Task<List<Address>> GetAddressesByCustomerId(int customerId);
        Task<Address> GetAddressById(int addressId);
        Task<bool> UpdateAddress(Address address);
        Task<bool> DeleteAddress(int addressId);

        Task<bool> UpdateAddressByCustomerId(int customerId, Address updatedAddress);
    }
}
