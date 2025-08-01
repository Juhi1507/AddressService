using WalletService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WalletService.Data
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options) { }

        public DbSet<Wallet> Wallets { get; set; }
    }
}
