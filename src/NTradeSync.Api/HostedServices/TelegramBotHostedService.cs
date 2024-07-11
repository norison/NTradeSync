using Microsoft.Extensions.Options;

using NTradeSync.Infrastructure.Options;

using OpenAPI.Net;
using OpenAPI.Net.Auth;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace NTradeSync.Api.HostedServices;

public sealed class TelegramBotHostedService(
    ILogger<TelegramBotHostedService> logger,
    ITelegramBotClient client,
    IOptions<CTraderOptions> options) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var me = await client.GetMeAsync(cancellationToken);

        logger.LogInformation("Telegram Bot {BotName} started", me.Username);

        var commands = new List<BotCommand> { new() { Command = "/start", Description = "Start the bot" } };

        await client.SetMyCommandsAsync(commands, cancellationToken: cancellationToken);

        client.StartReceiving(OnUpdateAsync, OnErrorAsync, cancellationToken: cancellationToken);
    }

    private async Task OnUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        if (update.Type is not UpdateType.Message || update.Message?.Text is null or not "/start")
        {
            return;
        }
        
        logger.LogInformation("Received a message from {Username}", update.Message.Chat.Username);

        var app = new App(options.Value.ClientId, options.Value.ClientSecret, "https://localhost:5001/");
        
        var url = app.GetAuthUri(Scope.Accounts).ToString();
        
        try
        {
            var keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithUrl("Login", url) } });
        
            await client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Click the button below to login and grand access to your account",
                replyMarkup: keyboard,
                cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An error occurred while sending a message");
        }
    }

    private async Task OnErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        logger.LogError(exception, "An error occurred while receiving an update");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping Telegram Bot");
        await client.CloseAsync(cancellationToken);
    }
}