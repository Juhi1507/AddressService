using System.Collections.Generic;
using System.Net;
using AddressService.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressService.Data
{
    public class AddressDbContext : DbContext
    {
        public AddressDbContext(DbContextOptions<AddressDbContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
    }
}
