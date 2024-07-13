using System.ComponentModel.DataAnnotations;

namespace NTradeSync.Infrastructure.Options;

public class TelegramBotOptions
{
    [Required] public string Token { get; set; } = string.Empty;
}