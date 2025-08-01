using WalletService.Data;
using WalletService.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace WalletService.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletDbContext _context;

        public WalletRepository(WalletDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet> GetWalletByCustomerId(int customerId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.CustomerID == customerId);
        }

        public async Task<Wallet> CreateWallet(int customerId)
        {
            var wallet = new Wallet
            {
                CustomerID = customerId,
                Balance = 0
            };

            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }

        public async Task<bool> AddFunds(int customerId, decimal amount)
        {
            var wallet = await GetWalletByCustomerId(customerId);
            if (wallet == null) return false;

            wallet.Balance += amount;
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> DeductFunds(int customerId, decimal amount)
        {
            var wallet = await GetWalletByCustomerId(customerId);
            if (wallet == null || wallet.Balance < amount) return false;

            wallet.Balance -= amount;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConfirmPayment(int customerId, decimal amount)
        {
            var wallet = await GetWalletByCustomerId(customerId);
            if (wallet == null) return false;

            wallet.Balance += amount;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
