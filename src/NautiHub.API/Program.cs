using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NautiHub.API.Configurations;
using NautiHub.Commin.Configurations;
using NautiHub.Infrastructure.DataContext;
using NautiHub.Infrastructure.Identity;

// Carregar variáveis do arquivo .env
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Configurar localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "pt-BR", "en-US" };
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR");
    options.SupportedCultures = supportedCultures.Select(c => new System.Globalization.CultureInfo(c)).ToList();
    options.SupportedUICultures = supportedCultures.Select(c => new System.Globalization.CultureInfo(c)).ToList();
    options.RequestCultureProviders = new List<Microsoft.AspNetCore.Localization.IRequestCultureProvider>
    {
        new Microsoft.AspNetCore.Localization.QueryStringRequestCultureProvider(),
        new Microsoft.AspNetCore.Localization.CookieRequestCultureProvider(),
        new Microsoft.AspNetCore.Localization.AcceptLanguageHeaderRequestCultureProvider()
    };
});

// Configurar serviços
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    // Obter connection string - priorizando .env e variáveis de ambiente
    var connectionString = Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION") 
        ?? builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Host=localhost;Port=5432;Database=nauti-hub-db;Username=postgres;Password=admin";
    
    options.UseNpgsql(connectionString);
});

builder.Services.AddIdentity<UserIdentity, IdentityRole<Guid>>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<DatabaseContext>();

// Registrar serviços de identidade personalizados
builder.Services.AddScoped<NautiHub.Core.Interfaces.IIdentityUserService, NautiHub.Infrastructure.Services.Identity.IdentityUserService>();
builder.Services.AddScoped<NautiHub.Core.Interfaces.IAuthenticationTokenService, NautiHub.Infrastructure.Services.Identity.AuthenticationTokenService>();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddControllers();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCommonServicesConfiguration(builder.Configuration);
builder.Services.AddRefitsConfiguration();
builder.Services.AddEvents();

// Configurar Identity manualmente - usando implementação simples para evitar dependência circular
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<NautiHub.Core.DomainObjects.INautiHubIdentity>(provider => 
{
    var contextAccessor = provider.GetService<IHttpContextAccessor>();
    return new SimpleNautiHubIdentity(contextAccessor);
});

var app = builder.Build();

// Configurar o pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configurar localization middleware
app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();

// Usar middlewares com namespace correto
app.UseMiddleware<NautiHub.Common.Middlewares.RequestIdMiddleware>();
app.UseMiddleware<NautiHub.API.Middlewares.AutenticacaoMiddleware>();

app.MapControllers();

// Aplicar migrações do banco de dados e criar roles padrão
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<NautiHub.Infrastructure.Services.Identity.RoleSeeder>>();
    
    // Aplicar migrações
    dbContext.Database.Migrate();
    
    // Criar roles padrão
    var roleSeeder = new NautiHub.Infrastructure.Services.Identity.RoleSeeder(roleManager, logger);
    await roleSeeder.SeedRolesAsync();
}

app.Run();