FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia projetos
COPY ArchEcommerceSystem.Core/*.csproj ArchEcommerceSystem.Core/
COPY ArchEcommerceSystem.UseCases/*.csproj ArchEcommerceSystem.UseCases/
COPY ArchEcommerceSystem.Infrastructure/*.csproj ArchEcommerceSystem.Infrastructure/
COPY ArchEcommerceSystem.WebApi/*.csproj ArchEcommerceSystem.WebApi/

# Restore (AGORA CORRETO)
RUN dotnet restore ArchEcommerceSystem.WebApi/ArchEcommerceSystem.WebApi.csproj

# Copia tudo
COPY . .

# Build
WORKDIR /src/ArchEcommerceSystem.WebApi
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ArchEcommerceSystem.WebApi.dll"]