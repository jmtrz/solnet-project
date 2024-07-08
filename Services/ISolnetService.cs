using Solnet.Rpc.Models;
using Solnet.Wallet;

namespace solnet_project_demo.Services;

public interface ISolnetService
{
    (Account, string, string) CreateAccount();
    Task<ulong> GetBalanceAsync(string publicKey);
    Task RequestAirDropAsync(string publicKey, ulong amount);
    Task<List<SignatureStatusInfo>> GetTransactionHistoryAsync(string publicKey);
}
