# Guia de Internacionalização (i18n) - NautiHub Backend

## Visão Geral

Este projeto implementa internacionalização usando Resource Files do .NET com `IStringLocalizer<T>`, permitindo que as mensagens do sistema sejam exibidas em português (pt-BR) e inglês (en-US).

## Estrutura

### 1. Arquivos de Recursos

- **`Resources/Messages.resx`** - Português (idioma padrão)
- **`Resources/Messages.en.resx`** - Inglês

### 2. Serviço Centralizado

- **`Resources/MessagesService.cs`** - Serviço tipado para acesso às mensagens localizadas

## Como Usar

### Em Handlers

```csharp
public class BoatCreateFeatureHandler(
    DatabaseContext context,
    ILogger<BoatCreateFeatureHandler> logger,
    IBoatRepository boatRepository,
    INautiHubIdentity nautiHubIdentity,
    MessagesService messagesService // Injetar o serviço
) : FeatureHandler(context), IRequestHandler<BoatCreateFeature, FeatureResponse<Guid>>
{
    private readonly MessagesService _messagesService = messagesService;

    public async Task<FeatureResponse<Guid>> Handle(BoatCreateFeature request, CancellationToken cancellationToken)
    {
        if (boatExists)
        {
            AddError(_messagesService.Boat_Already_Exists); // Usar mensagem localizada
            return new FeatureResponse<Guid>(ValidationResult, false, HttpStatusCode.Conflict);
        }
        
        // ...
    }
}
```

### Em Controllers

```csharp
public class AccountController : ControllerBase
{
    private readonly MessagesService _messagesService;

    public AccountController(
        IIdentityUserService identityService,
        ILogger<AccountController> logger,
        MessagesService messagesService)
    {
        _messagesService = messagesService;
    }

    public async Task<IActionResult> RegisterAsync([FromBody] RegisterInputModel input)
    {
        try
        {
            // ...
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _messagesService.Error_Registering_User);
            return StatusCode(500, _messagesService.Error_Internal_Server);
        }
    }
}
```

## Configuração de Idioma

O sistema suporta três formas de definir o idioma:

### 1. Query String
```
?culture=en-US
?ui-culture=en-US
```

### 2. Cookie
```
.AspNetCore.Culture=c=en-US|uic=en-US
```

### 3. Header HTTP
```
Accept-Language: en-US,en;q=0.9
```

## Padrão de Nomenclatura

As chaves seguem o formato: `[Entidade]_[Ação]`

Exemplos:
- `Boat_Already_Exists` - Embarcação já existe
- `Auth_User_Created` - Usuário criado
- `Error_Internal_Server` - Erro interno do servidor

## Adicionando Novas Mensagens

1. **Adicionar ao Messages.resx (português):**
```xml
<data name="Nova_Chave" xml:space="preserve">
    <value>Nova mensagem em português</value>
</data>
```

2. **Adicionar ao Messages.en.resx (inglês):**
```xml
<data name="Nova_Chave" xml:space="preserve">
    <value>New message in English</value>
</data>
```

3. **Adicionar ao MessagesService.cs:**
```csharp
public string Nova_Chave => _localizer["Nova_Chave"];
```

## Idiomas Suportados

- **pt-BR** (Português Brasil) - Padrão
- **en-US** (Inglês Estados Unidos)

## Testando

Para testar diferentes idiomas, use:

```bash
# Português (padrão)
curl -X GET "http://localhost:5000/api/boats"

# Inglês via query string
curl -X GET "http://localhost:5000/api/boats?culture=en-US"

# Inglês via header
curl -X GET "http://localhost:5000/api/boats" \
  -H "Accept-Language: en-US"
```