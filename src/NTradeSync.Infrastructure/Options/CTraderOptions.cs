using System.ComponentModel.DataAnnotations;

namespace NTradeSync.Infrastructure.Options;

public class CTraderOptions
{
    [Required] public string ClientId { get; set; } = string.Empty;
    [Required] public string ClientSecret { get; set; } = string.Empty;
}