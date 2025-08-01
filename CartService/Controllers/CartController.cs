using CartService.Models;
using CartService.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCartItemsByCustomerId(int customerId)
        {
            return Ok(await _cartRepository.GetCartItemsByCustomerId(customerId));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItem cartItem)
        {
            if (cartItem == null) return BadRequest("Invalid cart item data.");

            bool added = await _cartRepository.AddToCart(cartItem);
            if (!added) return BadRequest("Failed to add item to cart.");

            return Ok("Item added to cart successfully.");
        }

        [HttpPut("update/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItemQuantity(int cartItemId, [FromBody] int quantity)
        {
            bool updated = await _cartRepository.UpdateCartItemQuantity(cartItemId, quantity);
            if (!updated) return NotFound("Cart item not found.");

            return Ok("Cart item quantity updated.");
        }

        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            bool removed = await _cartRepository.RemoveFromCart(cartItemId);
            if (!removed) return NotFound("Cart item not found.");

            return Ok("Cart item removed successfully.");
        }

        [HttpDelete("clear/{customerId}")]
        public async Task<IActionResult> ClearCart(int customerId)
        {
            bool cleared = await _cartRepository.ClearCart(customerId);
            if (!cleared) return NotFound("Cart is already empty.");

            return Ok("Cart cleared successfully.");
        }
    }
}
