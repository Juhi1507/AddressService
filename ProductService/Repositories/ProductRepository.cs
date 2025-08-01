using ProductService.Data;
using ProductService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<List<Product>> GetProductsByCategory(string category)
        {
            return await _context.Products
                .Where(p => p.Category.ToLower() == category.ToLower())
                .ToListAsync();
        }

        public async Task<Product> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ApplyDiscount(int productId, double discountPercentage)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            product.DiscountPercentage = discountPercentage; // Apply discount
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
