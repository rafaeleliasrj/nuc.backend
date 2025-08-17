# Use SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar arquivos do projeto (vazio por enquanto)
COPY . .

# Criar projeto Web API
RUN dotnet new webapi -n MyApi -o MyApi --no-restore

WORKDIR /app/MyApi
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/MyApi/out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "MyApi.dll"]
