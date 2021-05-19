#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/StockAdapters/BitfinexAdapter/BitfinexAdapter.csproj", "src/StockAdapters/BitfinexAdapter/"]
COPY ["src/Infrastructure/Models/Models.csproj", "src/Infrastructure/Models/"]
COPY ["src/Robo.Database/Robo.Database.csproj", "src/Robo.Database/"]
COPY ["src/Infrastructure/Connectors/Connectors.csproj", "src/Infrastructure/Connectors/"]
COPY ["src/StockAdapters/BitfinexAdapter.Logic/BitfinexAdapter.Logic.csproj", "src/StockAdapters/BitfinexAdapter.Logic/"]
COPY ["src/Infrastructure/Scheduler/Scheduler.csproj", "src/Infrastructure/Scheduler/"]
COPY ["Infrastructure/Bitfinex.Net/Bitfinex.Net.csproj", "Infrastructure/Bitfinex.Net/"]
COPY ["Infrastructure/CryptoExchange.Net/CryptoExchange.Net.csproj", "Infrastructure/CryptoExchange.Net/"]
COPY ["src/Infrastructure/CommonLogger/CommonLogger.csproj", "src/Infrastructure/CommonLogger/"]
COPY ["src/Infrastructure/TelegramRoboBot/TelegramRoboBot.csproj", "src/Infrastructure/TelegramRoboBot/"]
RUN dotnet restore "src/StockAdapters/BitfinexAdapter/BitfinexAdapter.csproj"
COPY . .
WORKDIR "/src/src/StockAdapters/BitfinexAdapter"
RUN dotnet build "BitfinexAdapter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BitfinexAdapter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BitfinexAdapter.dll"]