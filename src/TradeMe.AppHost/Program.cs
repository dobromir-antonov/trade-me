var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("trademe-db")
    .WithDataVolume()
    .WithPgAdmin();

builder
    .AddProject<Projects.TradeMe_Api>("trademe-api")
    .WithReference(postgres);

builder.Build().Run();
