# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /app

# Copia csproj e restaura dependências (incluindo Core)
COPY ./src/NautiHub.API/*.csproj ./NautiHub.API/
COPY ./src/NautiHub.Core/*.csproj ./NautiHub.Core/
COPY ./src/NautiHub.Domain/*.csproj ./NautiHub.Domain/
COPY ./src/NautiHub.Infrastructure/*.csproj ./NautiHub.Infrastructure/
COPY ./src/NautiHub.Application/*.csproj ./NautiHub.Application/

RUN dotnet restore ./NautiHub.API/NautiHub.API.csproj

# Copia todo o código
COPY ./src .

# Publica a aplicação em Release
RUN dotnet publish ./NautiHub.API/NautiHub.API.csproj -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

# Copia o build da etapa anterior
COPY --from=build /app/out .

# Porta que a API vai expor
EXPOSE 8080

# Comando para rodar a API
ENTRYPOINT ["dotnet", "NautiHub.API.dll"]
