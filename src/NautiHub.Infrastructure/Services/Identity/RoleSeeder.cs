using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NautiHub.Domain.Enums;
using NautiHub.Infrastructure.Identity;

namespace NautiHub.Infrastructure.Services.Identity;

/// <summary>
/// Serviço responsável por criar as roles padrão do sistema no banco de dados
/// </summary>
public class RoleSeeder
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ILogger<RoleSeeder> _logger;

    public RoleSeeder(
        RoleManager<IdentityRole<Guid>> roleManager,
        ILogger<RoleSeeder> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    /// <summary>
    /// Cria todas as roles padrão do sistema se elas não existirem
    /// </summary>
    public async Task SeedRolesAsync()
    {
        try
        {
            // Mapear o enum UserType para strings de roles
            var roles = new[]
            {
                UserType.Admin.ToString(),
                UserType.Guest.ToString(),
                UserType.Host.ToString(),
                UserType.Both.ToString()
            };

            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole<Guid>(roleName);
                    var result = await _roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Role {RoleName} criada com sucesso", roleName);
                    }
                    else
                    {
                        var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                        _logger.LogError("Erro ao criar role {RoleName}: {Errors}", roleName, errors);
                    }
                }
                else
                {
                    _logger.LogDebug("Role {RoleName} já existe", roleName);
                }
            }

            _logger.LogInformation("Seed de roles concluído com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante o seed de roles");
            throw;
        }
    }
}