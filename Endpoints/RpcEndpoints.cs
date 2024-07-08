using solnet_project_demo.Handler;

namespace solnet_project_demo.Endpoints;

public static class RpcEndpoints
{
    public static RouteGroupBuilder SolanaGroupEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("balance", SolanaHandler.GetBalance);

        group.MapGet("transaction-history", SolanaHandler.TransactionHistory);

        group.MapPost("create-account", SolanaHandler.CreateAccount);

        return group;
    }
}

