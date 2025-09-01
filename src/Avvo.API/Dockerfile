# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copia csproj e restaura dependências (incluindo Core)
COPY ./src/Avvo.API/*.csproj ./Avvo.API/
COPY ./src/Avvo.Core/*.csproj ./Avvo.Core/
COPY ./src/Avvo.Domain/*.csproj ./Avvo.Domain/

RUN dotnet restore ./Avvo.API/Avvo.API.csproj

# Copia todo o código
COPY ./src .

# Publica a aplicação em Release
RUN dotnet publish ./Avvo.API/Avvo.API.csproj -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copia o build da etapa anterior
COPY --from=build /app/out .

# Porta que a API vai expor
EXPOSE 5000

# Comando para rodar a API
ENTRYPOINT ["dotnet", "Avvo.API.dll"]
