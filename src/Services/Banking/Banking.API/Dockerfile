# Dockerfile — production-ready cho Banking.API

# ─────────────────────────────────────────────────────────────
# BUILD STAGE
# ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# TRICK 1: Copy project files trước để tận dụng Docker layer cache
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]

COPY ["src/Services/Banking/Banking.API/Banking.API.csproj", "src/Services/Banking/Banking.API/"]
COPY ["src/BuildingBlocks/Banking.Contracts/Banking.Contracts.csproj", "src/BuildingBlocks/Banking.Contracts/"]
COPY ["src/BuildingBlocks/Banking.Logging/Banking.Logging.csproj", "src/BuildingBlocks/Banking.Logging/"]
COPY ["src/Services/Banking/Banking.Application/Banking.Application.csproj", "src/Services/Banking/Banking.Application/"]
COPY ["src/Services/Banking/Banking.Domain/Banking.Domain.csproj", "src/Services/Banking/Banking.Domain/"]
COPY ["src/Services/Banking/Banking.Infrastructure/Banking.Infrastructure.csproj", "src/Services/Banking/Banking.Infrastructure/"]
COPY ["src/Services/Banking/Banking.Persistence/Banking.Persistence.csproj", "src/Services/Banking/Banking.Persistence/"]

# Restore dependencies
RUN dotnet restore "./src/Services/Banking/Banking.API/Banking.API.csproj" \
    --runtime linux-x64 \
    /p:PublishReadyToRun=true

# Copy toàn bộ source sau khi restore
COPY . .

WORKDIR "/src/src/Services/Banking/Banking.API"

# Publish optimized build
RUN dotnet publish "./Banking.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -r linux-x64 \
    --no-restore \
    --self-contained false \
    -o /app/publish \
    /p:UseAppHost=false

# ─────────────────────────────────────────────────────────────
# RUNTIME STAGE
# ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# TRICK 2: Chạy bằng non-root user
RUN groupadd -g 1001 appgroup \
    && useradd -u 1001 -g appgroup -s /bin/false appuser \
    && chown -R appuser:appgroup /app

# TRICK 3: Chỉ copy artifact cần thiết
COPY --from=build --chown=appuser:appgroup /app/publish .

# TRICK 4: Healthcheck
# Đổi endpoint nếu project bạn không có endpoint này
HEALTHCHECK --interval=30s --timeout=10s --start-period=30s --retries=3 \
    CMD wget -qO- http://localhost:8080/health/live || exit 1

# Security best practice
USER appuser

# ASP.NET Core config
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose app port
EXPOSE 8080

ENTRYPOINT ["dotnet", "Banking.API.dll"]