FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/NTradeSync.Api/NTradeSync.Api.csproj", "src/NTradeSync.Api/"]
COPY ["src/NTradeSync.Persistence/NTradeSync.Persistence.csproj", "src/NTradeSync.Persistence/"]
COPY ["src/NTradeSync.Application/NTradeSync.Application.csproj", "src/NTradeSync.Application/"]
COPY ["src/NTradeSync.Infrastructure/NTradeSync.Infrastructure.csproj", "src/NTradeSync.Infrastructure/"]
RUN dotnet restore "src/NTradeSync.Api/NTradeSync.Api.csproj"
COPY . .
WORKDIR "/src/src/NTradeSync.Api"
RUN dotnet build "NTradeSync.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NTradeSync.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NTradeSync.Api.dll"]
