using CartService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CartService.Repositories
{
    public interface ICartRepository
    {
        Task<List<CartItem>> GetCartItemsByCustomerId(int customerId);
        Task<bool> AddToCart(CartItem cartItem);
        Task<bool> UpdateCartItemQuantity(int cartItemId, int quantity);
        Task<bool> RemoveFromCart(int cartItemId);
        Task<bool> ClearCart(int customerId);
    }
}
