using NTradeSync.Api.Extensions;
using NTradeSync.Api.HostedServices;

using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<TelegramBotHostedService>();
builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(builder.Configuration["TelegramBot:Token"]));

builder.AddApi().AddInfrastructure();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.RunAsync();