
using Solnet.Rpc;
using Solnet.Rpc.Models;
using Solnet.Wallet;
using Solnet.Wallet.Bip39;
using solnet_project_demo.Services;

namespace SolNet.Services;

public class SolNetService : ISolnetService
{
    // Add your service methods here
    private readonly IRpcClient _rpcClient;

    public SolNetService()
    {
        _rpcClient = ClientFactory.GetClient(Cluster.DevNet);
    }

    public (Account, string, string) CreateAccount()
    {        
        var mnemonic = new Mnemonic(WordList.English,WordCount.Twelve);
        var wallet = new Wallet(mnemonic);
        var account = wallet.Account;
        return (account, Convert.ToBase64String(account.PrivateKey.KeyBytes), mnemonic.ToString());
    }

    public async Task<ulong> GetBalanceAsync(string publicKey)
    {
        var balanceResult = await _rpcClient.GetBalanceAsync(new PublicKey(publicKey));

        if(balanceResult.WasSuccessful)
        {
            return balanceResult.Result.Value;
        }

        throw new Exception(balanceResult.Reason);
    }

    public async Task RequestAirDropAsync(string publicKey, ulong amount)
    {
        var airdropResult = await _rpcClient.RequestAirdropAsync(new PublicKey(publicKey), amount);
        if(!airdropResult.WasSuccessful)
        {
            throw new Exception(airdropResult.Reason);
        }
        await ConfirmTransactionAsync(airdropResult.Result);
    }

    private async Task ConfirmTransactionAsync(string transactionSignature)
    {
        const int maxAttempts = 10;
        const int delay = 2000;
        for (int i = 0; i< maxAttempts; i++)
        {
            var confirmation = await _rpcClient.GetSignatureStatusesAsync(new[] { transactionSignature }.ToList());
            if (confirmation.WasSuccessful && confirmation.Result.Value[0] != null)
            {
                if (confirmation.Result.Value[0].ConfirmationStatus == "confirmed" ||
                        confirmation.Result.Value[0].ConfirmationStatus == "finalized")
                {
                    return;
                }
            }
            await Task.Delay(delay);
        }
        throw new Exception("Transaction confirmation failed.");
    }

    public async Task<List<SignatureStatusInfo>> GetTransactionHistoryAsync(string publicKey)
    {
        var transactionHistory = await _rpcClient.GetSignaturesForAddressAsync(new PublicKey(publicKey));
        if(transactionHistory.WasSuccessful)
        {
            return transactionHistory.Result;
        }
        throw new Exception(transactionHistory.Reason);
    }
}
