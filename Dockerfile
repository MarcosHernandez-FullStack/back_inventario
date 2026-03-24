# ── Etapa 1: Build ──────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY BackInventario.Domain/BackInventario.Domain.csproj             BackInventario.Domain/
COPY BackInventario.Application/BackInventario.Application.csproj   BackInventario.Application/
COPY BackInventario.Infrastructure/BackInventario.Infrastructure.csproj BackInventario.Infrastructure/
COPY BackInventario.API/BackInventario.API.csproj                   BackInventario.API/

RUN dotnet restore BackInventario.API/BackInventario.API.csproj

# Copiar el resto del código y publicar
COPY . .
RUN dotnet publish BackInventario.API/BackInventario.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ── Etapa 2: Runtime ─────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "BackInventario.API.dll"]
