# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and restore (layer cache optimization)
COPY FinanceApp.sln .
COPY src/FinanceApp.Domain/FinanceApp.Domain.csproj src/FinanceApp.Domain/
COPY src/FinanceApp.Application/FinanceApp.Application.csproj src/FinanceApp.Application/
COPY src/FinanceApp.Infrastructure/FinanceApp.Infrastructure.csproj src/FinanceApp.Infrastructure/
COPY src/FinanceApp.API/FinanceApp.API.csproj src/FinanceApp.API/

RUN dotnet restore src/FinanceApp.API/FinanceApp.API.csproj

# Copy all source and build
COPY src/ src/
RUN dotnet publish src/FinanceApp.API/FinanceApp.API.csproj \
    -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create non-root user
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
USER appuser

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "FinanceApp.API.dll"]
