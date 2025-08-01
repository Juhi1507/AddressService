using WalletService.Models;
using System.Threading.Tasks;

namespace WalletService.Repositories
{
    public interface IWalletRepository
    {
        Task<Wallet> CreateWallet(int customerId);
        Task<Wallet> GetWalletByCustomerId(int customerId);
        Task<bool> AddFunds(int customerId, decimal amount);
        Task<bool> DeductFunds(int customerId, decimal amount);
        Task<bool> ConfirmPayment(int customerId, decimal amount);
    }
}
