using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AddressService.Models;
using AddressService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AddressService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;

        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] Address address)
        {
            if (address == null)
                return BadRequest("Invalid address data.");

            var newAddress = await _addressRepository.AddAddress(address);
            return Ok(newAddress);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<Address>>> GetAddressesByCustomerId(int customerId)
        {
            return Ok(await _addressRepository.GetAddressesByCustomerId(customerId));
        }

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetAddressById(int addressId)
        {
            var address = await _addressRepository.GetAddressById(addressId);
            if (address == null) return NotFound("Address not found.");
            return Ok(address);
        }

        [HttpPut("{addressId}")]
        public async Task<IActionResult> UpdateAddress(int addressId, [FromBody] Address address)
        {
            if (address.AddressID != addressId)
                return BadRequest("Address ID mismatch.");

            bool updated = await _addressRepository.UpdateAddress(address);
            if (!updated) return NotFound("Address not found.");

            return Ok("Address updated successfully.");
        }

        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            bool deleted = await _addressRepository.DeleteAddress(addressId);
            if (!deleted) return NotFound("Address not found.");

            return Ok("Address deleted successfully.");
        }

        [HttpPut("customer/{customerId}")]
        public async Task<IActionResult> UpdateAddressByCustomerId(int customerId, [FromBody] Address updatedAddress)
        {
            if (updatedAddress == null)
                return BadRequest("Invalid Address Data");

            bool isUpdated = await _addressRepository.UpdateAddressByCustomerId(customerId, updatedAddress);

            if (!isUpdated)
                return NotFound($"No address found for customer ID {customerId}");

            return Ok("Address updated successfully.");
        }

    }
}
