# 🌊 NautiHub API

Backend moderno construído com **.NET 10** seguindo **Domain-Driven Design (DDD)**, com arquitetura limpa, escalabilidade e integração com serviços cloud.

---

## 📑 Sumário

- [🚀 Stack Tecnológica](#-stack-tecnológica)
- [🏗️ Arquitetura](#️-arquitetura)
- [🚀 Setup Rápido](#-setup-rápido)
- [📋 Configurações Essenciais](#-configurações-essenciais)
- [🔧 Features Principais](#-features-principais)
- [🌍 Internacionalização (i18n)](#-internacionalização-i18n)
- [🧪 Testes](#-testes)
- [📚 Endpoints Principais](#-endpoints-principais)
- [🔄 Migrations](#-migrations)
- [📋 Padrões e Convenções](#-padrões-e-convenções)
- [🛠️ Ambiente de Desenvolvimento](#️-ambiente-de-desenvolvimento)
- [📈 Performance](#-performance)
- [🔐 Segurança](#-segurança)
- [📞 Suporte](#-suporte)

## 🚀 Stack Tecnológica

### Core
- **.NET 10** - Framework principal
- **Domain-Driven Design (DDD)** - Arquitetura orientada ao domínio
- **MediatR** - Padrão CQRS e desacoplamento
- **Entity Framework Core 9.0** - ORM com Pomelo MySQL

### Infraestrutura & Cloud
- **MySQL 8.0** - Banco de dados relacional
- **AWS S3** - Armazenamento de arquivos
- **AWS SQS** - Mensageria assíncrona
- **Redis** - Cache e sessões
- **Hangfire** - Background jobs com MySQL

### Validação & Documentação
- **FluentValidation** - Validação robusta
- **Swagger/OpenAPI** - Documentação automática
- **MicroElements.Swashbuckle.FluentValidation** - Integração Swagger + FluentValidation

### Comunicação & Utilitários
- **FluentEmail** - Envio de emails (SMTP/SendGrid)
- **Twilio** - SMS
- **Refit** - Client HTTP fortemente tipado
- **DinkToPdf** - Geração de PDFs
- **SixLabors.ImageSharp** - Processamento de imagens
- **PdfPig** - Leitura de PDFs

### Autenticação & Segurança
- **JWT Bearer** - Autenticação
- **ASP.NET Core Identity** - Gerenciamento de usuários

---

## 🏗️ Arquitetura

```
src/
├── NautiHub.API/              # 🔌 Endpoints HTTP, Swagger, DI
├── NautiHub.Application/      # ⚙️ Casos de uso, Commands, Queries, Handlers
├── NautiHub.Domain/           # 🧠 Entidades, Value Objects, Regras de negócio
├── NautiHub.Infrastructure/    # 🏗️ EF Core, Repositories, AWS, MySQL
├── NautiHub.Core/             # 📦 Contratos, Interfaces compartilhadas
└── NautiHub.CrossCutting/     # 🛠️ Serviços transversais, Email, PDF
```

### Fluxo de Requisição

```
API Controller → MediatR → Handler → Domain Logic → Infrastructure → Database/Cloud
```

---

## 🚀 Setup Rápido

### Pré-requisitos
- .NET SDK 10
- Docker & Docker Compose
- MySQL Client (opcional)
- Conta AWS (S3, SQS)

### Ambiente

1. **Clonar o repositório**
```bash
git clone <repositorio>
cd NautiHub.Backend
```

2. **Configurar ambiente**
```bash
# Copiar template de variáveis
cp src/NautiHub.API/appsettings.Development.json.example src/NautiHub.API/appsettings.Development.json
# Editar com suas configurações
```

3. **Subir infraestrutura**
```bash
docker-compose up -d mysql
```

4. **Executar aplicação**
```bash
dotnet restore
dotnet build
dotnet run --project src/NautiHub.API
```

### Docker Compose (Ambiente de Desenvolvimento)

```yaml
version: "3.1"
services:
  mysql:
    image: mysql:8.0
    command: --lower_case_table_names=1
    environment:
      MYSQL_ROOT_PASSWORD: admin
      MYSQL_DATABASE: nauti_hub
    ports:
      - "3306:3306"
    volumes:
      - mysql_data:/var/lib/mysql

volumes:
  mysql_data:
```

---

## 📋 Configurações Essenciais

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=nauti_hub;User=root;Password=admin;",
    "Redis": "localhost:6379"
  },
  "AWS": {
    "Region": "us-east-1",
    "AccessKey": "sua-access-key",
    "SecretKey": "sua-secret-key"
  },
  "S3": {
    "BucketName": "nautihub-files"
  },
  "SQS": {
    "QueueUrl": "sua-queue-url"
  },
  "EmailSettings": {
    "FromEmail": "noreply@nautihub.com",
    "SmtpServer": "smtp.gmail.com",
    "Port": 587
  },
  "Jwt": {
    "SecretKey": "sua-chave-secreta-jwt",
    "ExpiryMinutes": 1440
  }
}
```

---

## 🔧 Features Principais

### ✅ Validação Centralizada
- **FluentValidation** integrado ao pipeline do MediatR
- Validação automática exposta no Swagger
- Reutilização de regras de validação

### 🌍 Internacionalização Completa
- Suporte a múltiplos idiomas com .resx
- Detecção automática de cultura (query string, cookie, header)
- Mensagens centralizadas com type safety
- Fácil extensão para novos idiomas

### 📦 Armazenamento na AWS
- Upload/download via S3
- URLs pré-assinadas para acesso seguro
- Processamento de imagens com ImageSharp

### 📬 Mensageria Assíncrona
- SQS para processamento em background
- Eventos de domínio desacoplados
- Workers dedicados para consumo

### 📊 Background Jobs
- Hangfire com MySQL storage
- Dashboard autenticado
- Jobs recorrentes e agendados

### 📧 Comunicação
- Emails via SMTP/SendGrid com templates Razor
- SMS via Twilio
- PDFs com DinkToPdf

---

## 🧪 Testes

```bash
# Executar todos os testes
dotnet test

# Executar com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Testes específicos por projeto
dotnet test tests/NautiHub.UnitTests/
dotnet test tests/NautiHub.IntegrationTests/
```

---

## 📚 Endpoints Principais

Acesse `https://localhost:5000/swagger` para documentação completa.

### Autenticação
- `POST /api/auth/login`
- `POST /api/auth/register`
- `POST /api/auth/refresh-token`

### Recursos
- `GET /api/users` - Listagem paginada
- `POST /api/files/upload` - Upload de arquivos
- `GET /api/files/{id}/download` - Download com URL pré-assinada

---

## 🔄 Migrations

```bash
# Criar migration
dotnet ef migrations add AddNewEntity --project src/NautiHub.Infrastructure --startup-project src/NautiHub.API

# Aplicar migrations
dotnet ef database update --project src/NautiHub.Infrastructure --startup-project src/NautiHub.API
```

---

## 🌍 Internacionalização (i18n)

O projeto possui suporte completo a múltiplos idiomas utilizando o sistema de localização do ASP.NET Core com arquivos de recursos (.resx).

### 📚 Arquitetura de Tradução

```
src/NautiHub.Core/Resources/
├── Messages.resx          # 🇧🇷 Português Brasileiro (padrão)
├── Messages.en.resx        # 🇺🇸 Inglês
└── MessagesService.cs      # 📦 Serviço centralizado de mensagens
```

### 🎯 Idiomas Suportados

- **pt-BR** (Português Brasileiro) - Idioma padrão
- **en-US** (Inglês Americano)
- *[Fácil extensão para novos idiomas]*

### 🔄 Como Funciona

1. **Detecção Automática**: Query string, cookie ou header `Accept-Language`
2. **Fallback**: Retorna para português se tradução não encontrada
3. **Centralização**: Todas as mensagens em `MessagesService`
4. **Type Safety**: Propriedades tipadas evitam "magic strings"

### 📋 Fluxo de Inclusão de Novas Traduções

**⚠️ Ordem Obrigatória dos Artefatos:**

#### **1️⃣ MessagesService.cs** - Adicionar Propriedades
```csharp
// src/NautiHub.Core/Resources/MessagesService.cs

// Na seção apropriada (Auth, Validation, Error, etc.)
public string Validation_Required_Field => _localizer["Validation_Required_Field"];
public string Validation_Invalid_Email => _localizer["Validation_Invalid_Email"];
```

**Regras de Nomenclatura:**
- ✅ PascalCase com underscore: `Auth_User_Not_Found`
- ✅ Prefixo por categoria: `Auth_`, `Validation_`, `Error_`, `Boat_`
- ❌ Não usar espaços ou caracteres especiais

---

#### **2️⃣ Messages.resx** - Português (Base)
```xml
<!-- src/NautiHub.Core/Resources/Messages.resx -->

<data name="Validation_Required_Field" xml:space="preserve">
  <value>Campo obrigatório.</value>
</data>
<data name="Validation_Invalid_Email" xml:space="preserve">
  <value>E-mail inválido.</value>
</data>
```

---

#### **3️⃣ Messages.en.resx** - Inglês
```xml
<!-- src/NautiHub.Core/Resources/Messages.en.resx -->

<data name="Validation_Required_Field" xml:space="preserve">
  <value>Required field.</value>
</data>
<data name="Validation_Invalid_Email" xml:space="preserve">
  <value>Invalid email.</value>
</data>
```

**⚠️ Importante:** O `name` deve ser **IDÊNTICO** em ambos os arquivos.

---

#### **4️⃣ ResponseErrorMessages.cs** - Mapeamento
```csharp
// src/NautiHub.Core/Communication/ResponseErrorMessages.cs

return messageKey switch
{
    // ... mensagens existentes ...
    "Validation_Required_Field" => _messagesService.Validation_Required_Field,
    "Validation_Invalid_Email" => _messagesService.Validation_Invalid_Email,
    _ => messageKey // Fallback para chave original
};
```

---

#### **5️⃣ Usar no Código**
```csharp
// Em Handlers, Controllers, Services

// Injeção no construtor
public MeuHandler(MessagesService messagesService)
{
    _messagesService = messagesService;
}

// Usar a mensagem localizada
return (false, _messagesService.Validation_Required_Field);
```

### 🚀 Adicionando Novo Idioma (Ex: Espanhol)

1. **Criar arquivo**: `Messages.es.resx`
2. **Copiar estrutura** do `Messages.resx`
3. **Traduzir valores** mantendo os `name` idênticos
4. **Atualizar Program.cs**:
   ```csharp
   var supportedCultures = new[] { "pt-BR", "en-US", "es-ES" };
   ```

### 🧪 Testando Traduções

```bash
# Teste em português (padrão)
GET /api/account/test-localization

# Teste em inglês
GET /api/account/test-localization?culture=en-US
# ou com header: Accept-Language: en-US

# Teste em espanhol (se disponível)
GET /api/account/test-localization?culture=es-ES
```

### ⚠️ Problemas Comuns

| Problema | Solução |
|----------|---------|
| Retorna chave em vez do valor | Verifique se o `name` no .resx está exatamente igual |
| Erro de build nos recursos | Confira se não há caracteres especiais no XML |
| Tradução não funciona | Verifique se a cultura está em `supportedCultures` no Program.cs |
| Mensagens duplicadas | Não adicione `EmbeddedResource` manualmente - .NET SDK inclui automaticamente |

### 📝 Exemplo Completo - Mensagem de CPF Inválido

**1. MessagesService.cs:**
```csharp
public string Validation_Invalid_CPF => _localizer["Validation_Invalid_CPF"];
```

**2. Messages.resx:**
```xml
<data name="Validation_Invalid_CPF" xml:space="preserve">
  <value>CPF inválido.</value>
</data>
```

**3. Messages.en.resx:**
```xml
<data name="Validation_Invalid_CPF" xml:space="preserve">
  <value>Invalid CPF.</value>
</data>
```

**4. ResponseErrorMessages.cs:**
```csharp
"Validation_Invalid_CPF" => _messagesService.Validation_Invalid_CPF,
```

**5. Uso:**
```csharp
if (!IsValidCPF(cpf))
    return BadRequest(_messagesService.Validation_Invalid_CPF);
```

---

## 📋 Padrões e Convenções

### ✅ Boas Práticas
- **Nomenclatura**: Classes e namespaces em inglês, documentação em português
- **Controllers**: Finos, apenas orquestração
- **Handlers**: Sem `new`, sempre com DI
- **Domain**: Sem dependências externas
- **Infrastructure**: Detalhes técnicos isolados

### 🚫 O Que Evitar
- Lógica de negócio em Controllers
- Conexões diretas ao banco fora da Infrastructure
- Código mágico (reflection, dinâmico)
- Estado global
- Lazy Loading no EF Core

---

## 🛠️ Ambiente de Desenvolvimento

### VS Code Extensions Recomendadas
- C# Dev Kit
- .NET Runtime Install Tool
- Docker
- GitLens

### Debug
- Configuração de launch.json inclusa
- Breakpoints funcionam em todas as camadas
- Hot reload disponível no modo de desenvolvimento

---

## 📈 Performance

### Cache
- Redis para cache distribuído
- Cache de resposta em Controllers
- Cache de consultas frequentes

### Otimizações
- Connection pooling MySQL
- Async/await em operações I/O
- Minimal allocations
- Proper DTOs para transferência

---

## 🔐 Segurança

- **JWT Bearer Authentication**
- **Password hashing** com ASP.NET Core Identity
- **CORS** configurado
- **Rate limiting** (opcional)
- **Input validation** centralizado
- **SQL injection prevention** via EF Core

---

## 📞 Suporte

- **Issues**: Criar ticket no GitHub
- **Documentação**: Ver pasta `/docs`
- **AGENTS.md**: Instruções específicas para agentes de IA

---

## 📜 Licença

[Adicionar informações de licença]

---

**Feito com ❤️ para a comunidade marítima**
