using ProductService.Models;
using ProductService.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Request.Headers["UserRole"].ToString();
            return role == "Admin"; //  Only allow Admin role
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _productRepository.GetAllProducts());
        }

        [HttpGet("GetProductById/{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var product = await _productRepository.GetProductById(productId);
            if (product == null) return NotFound("Product not found.");
            return Ok(product);
        }

        [HttpGet("GetProductsByCategory/{category}")]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            return Ok(await _productRepository.GetProductsByCategory(category));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            //if (!IsAdmin()) return Unauthorized("Access Denied! Only Admins can perform this action.");
            if (product == null) return BadRequest("Invalid product data.");

            var newProduct = await _productRepository.AddProduct(product);
            return CreatedAtAction(nameof(GetProductById), new { productId = newProduct.ProductID }, newProduct);
        }

        [HttpPut("UpdateProduct/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] Product product)
        {
            if (!IsAdmin()) return Unauthorized("Access Denied! Only Admins can perform this action.");
            if (product.ProductID != productId)
                return BadRequest("Product ID mismatch.");

            bool updated = await _productRepository.UpdateProduct(product);
            if (!updated) return NotFound("Product not found.");

            return Ok("Product updated successfully.");
        }

        [HttpDelete("DeleteProduct/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            if (!IsAdmin()) return Unauthorized("Access Denied! Only Admins can perform this action.");
            bool deleted = await _productRepository.DeleteProduct(productId);
            if (!deleted) return NotFound("Product not found.");

            return Ok("Product deleted successfully.");
        }

        [HttpPut("ApplyDiscount/{productId}/{discountPercentage}")]
        public async Task<IActionResult> ApplyDiscount(int productId, double discountPercentage)
        {
            if (discountPercentage < 0 || discountPercentage > 100)
                return BadRequest("Discount percentage must be between 0 and 100.");

            bool updated = await _productRepository.ApplyDiscount(productId, discountPercentage);
            if (!updated) return NotFound("Product not found.");

            return Ok($"Discount of {discountPercentage}% applied successfully.");
        }
    }
}
