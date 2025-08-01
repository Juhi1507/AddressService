using AddressService.Data;
using AddressService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AddressService.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AddressDbContext _context;

        public AddressRepository(AddressDbContext context)
        {
            _context = context;
        }

        public async Task<Address> AddAddress(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<List<Address>> GetAddressesByCustomerId(int customerId)
        {
            return await _context.Addresses
                .Where(a => a.CustomerID == customerId)
                .ToListAsync();
        }

        public async Task<Address> GetAddressById(int addressId)
        {
            return await _context.Addresses.FindAsync(addressId);
        }

        public async Task<bool> UpdateAddress(Address address)
        {
            _context.Addresses.Update(address);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAddress(int addressId)
        {
            var address = await _context.Addresses.FindAsync(addressId);
            if (address == null) return false;

            _context.Addresses.Remove(address);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAddressByCustomerId(int customerId, Address updatedAddress)
        {
            var existingAddress = await _context.Addresses
                .FirstOrDefaultAsync(a => a.CustomerID == customerId);

            if (existingAddress == null)
                return false; // No address found for the customer

            // Update address fields
            existingAddress.Street = updatedAddress.Street;
            existingAddress.City = updatedAddress.City;
            existingAddress.State = updatedAddress.State;
            existingAddress.ZipCode = updatedAddress.ZipCode;
            existingAddress.Country = updatedAddress.Country;
            existingAddress.Type = updatedAddress.Type;

            _context.Addresses.Update(existingAddress);
            await _context.SaveChangesAsync();
            return true;
        }



    }
}
