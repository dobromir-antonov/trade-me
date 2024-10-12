# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TradeMe.Api/TradeMe.Api.csproj", "src/TradeMe.Api/"]
COPY ["src/Modules/Orders/Modules.Orders.Infrastructure/Modules.Orders.Infrastructure.csproj", "src/Modules/Orders/Modules.Orders.Infrastructure/"]
COPY ["src/Modules/Orders/Modules.Orders.Application/Modules.Orders.Application.csproj", "src/Modules/Orders/Modules.Orders.Application/"]
COPY ["src/Modules/Price/Modules.Price.IntegrationEvents/Modules.Price.IntegrationEvents.csproj", "src/Modules/Price/Modules.Price.IntegrationEvents/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]
COPY ["src/Modules/Orders/Modules.Orders.Domain/Modules.Orders.Domain.csproj", "src/Modules/Orders/Modules.Orders.Domain/"]
COPY ["src/Modules/Portfolio/Modules.Portfolio.Infrastructure/Modules.Portfolio.Infrastructure.csproj", "src/Modules/Portfolio/Modules.Portfolio.Infrastructure/"]
COPY ["src/Modules/Portfolio/Modules.Portfolio.Application/Modules.Portfolio.Application.csproj", "src/Modules/Portfolio/Modules.Portfolio.Application/"]
COPY ["src/Modules/Portfolio/Modules.Portfolio.Domain/Modules.Portfolio.Domain.csproj", "src/Modules/Portfolio/Modules.Portfolio.Domain/"]
COPY ["src/Modules/Price/Modules.Price.Infrastructure/Modules.Price.Infrastructure.csproj", "src/Modules/Price/Modules.Price.Infrastructure/"]
COPY ["src/Modules/Price/Modules.Price.Domain/Modules.Price.Domain.csproj", "src/Modules/Price/Modules.Price.Domain/"]
RUN dotnet restore "./src/TradeMe.Api/TradeMe.Api.csproj"
COPY . .
WORKDIR "/src/src/TradeMe.Api"
RUN dotnet build "./TradeMe.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TradeMe.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradeMe.Api.dll"]