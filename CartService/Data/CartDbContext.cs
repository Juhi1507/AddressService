using System.Collections.Generic;
using CartService.Models;
using Microsoft.EntityFrameworkCore;

namespace CartService.Data
{
    public class CartDbContext : DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> options) : base(options) { }

        public DbSet<CartItem> CartItems { get; set; }
    }
}
