using CartService.Data;
using CartService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CartService.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartDbContext _context;

        public CartRepository(CartDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartItem>> GetCartItemsByCustomerId(int customerId)
        {
            return await _context.CartItems
                .Where(c => c.CustomerID == customerId)
                .ToListAsync();
        }

        public async Task<bool> AddToCart(CartItem cartItem)
        {
            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.CustomerID == cartItem.CustomerID && c.ProductID == cartItem.ProductID);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cartItem.Quantity;
            }
            else
            {
                _context.CartItems.Add(cartItem);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null) return false;

            cartItem.Quantity = quantity;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveFromCart(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null) return false;

            _context.CartItems.Remove(cartItem);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ClearCart(int customerId)
        {
            var cartItems = await _context.CartItems
                .Where(c => c.CustomerID == customerId)
                .ToListAsync();

            if (!cartItems.Any()) return false;

            _context.CartItems.RemoveRange(cartItems);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
