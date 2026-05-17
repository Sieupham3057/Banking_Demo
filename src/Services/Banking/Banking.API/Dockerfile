# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/Services/Banking/Banking.API/Banking.API.csproj", "src/Services/Banking/Banking.API/"]
COPY ["src/BuildingBlocks/Banking.Contracts/Banking.Contracts.csproj", "src/BuildingBlocks/Banking.Contracts/"]
COPY ["src/BuildingBlocks/Banking.Logging/Banking.Logging.csproj", "src/BuildingBlocks/Banking.Logging/"]
COPY ["src/Services/Banking/Banking.Application/Banking.Application.csproj", "src/Services/Banking/Banking.Application/"]
COPY ["src/Services/Banking/Banking.Domain/Banking.Domain.csproj", "src/Services/Banking/Banking.Domain/"]
COPY ["src/Services/Banking/Banking.Infrastructure/Banking.Infrastructure.csproj", "src/Services/Banking/Banking.Infrastructure/"]
COPY ["src/Services/Banking/Banking.Persistence/Banking.Persistence.csproj", "src/Services/Banking/Banking.Persistence/"]
RUN dotnet restore "./src/Services/Banking/Banking.API/Banking.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Banking/Banking.API"
RUN dotnet build "./Banking.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Banking.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Banking.API.dll"]