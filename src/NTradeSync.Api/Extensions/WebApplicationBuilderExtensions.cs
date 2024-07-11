using NTradeSync.Infrastructure.Options;

namespace NTradeSync.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        
        return builder;
    }
    
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<CTraderOptions>()
            .BindConfiguration("Terminals:CTrader")
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        return builder;
    }
}