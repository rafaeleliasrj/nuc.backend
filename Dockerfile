# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copia csproj e restaura dependências
COPY *.csproj ./
RUN dotnet restore

# Copia todo o código
COPY . .

# Publica a aplicação em Release
RUN dotnet publish -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copia o build da etapa anterior
COPY --from=build /app/out .

# Porta que a API vai expor
EXPOSE 5000

# Comando para rodar a API
ENTRYPOINT ["dotnet", "NucApi.dll"]
