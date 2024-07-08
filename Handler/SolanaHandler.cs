using Microsoft.AspNetCore.Mvc;
using Solnet.Wallet;
using SolNet.Services;
using solnet_project_demo.Services;
using System.Security.Cryptography.X509Certificates;

namespace solnet_project_demo.Handler;

public static class SolanaHandler
{
    public static async Task<IResult> GetBalance([FromQuery]string publicKey, [FromServices] ISolnetService solnetService)
    {
        try
        {
            return Results.Ok(await solnetService.GetBalanceAsync(publicKey));
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }
    }

    public static IResult CreateAccount([FromServices] ISolnetService solnetService)
    {
        try
        {
            return Results.Ok(solnetService.CreateAccount());
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> TransactionHistory([FromQuery] string publicKey,[FromServices] ISolnetService solnetService)
    {
        try
        {
            return Results.Ok(await solnetService.GetTransactionHistoryAsync(publicKey));
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
