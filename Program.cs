using solnet_project_demo.Extensions;

var builder = WebApplication.CreateBuilder(args);


var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

app.Run();