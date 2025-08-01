using ProductService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(int productId);
        Task<List<Product>> GetProductsByCategory(string category);
        Task<Product> AddProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int productId);

        Task<bool> ApplyDiscount(int productId, double discountPercentage);
    }
}
