using NTradeSync.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApi().AddInfrastructure();

var app = builder.Build();

await app.RunAsync();