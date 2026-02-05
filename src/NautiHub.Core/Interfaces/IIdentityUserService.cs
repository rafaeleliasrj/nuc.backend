using NautiHub.Core.DTOs;

namespace NautiHub.Core.Interfaces;

/// <summary>
/// Interface para serviços de identidade do usuário
/// Abstrai as operações com ASP.NET Core Identity
/// </summary>
public interface IIdentityUserService
{
    /// <summary>
    /// Registra um novo usuário no sistema
    /// </summary>
    /// <param name="email">Email do usuário</param>
    /// <param name="password">Senha do usuário</param>
    /// <param name="firstName">Primeiro nome</param>
    /// <param name="lastName">Sobrenome</param>
    /// <returns>Tupla com sucesso e mensagem de erro/sucesso</returns>
    Task<(bool Success, string Message, string? UserId)> RegisterUserAsync(string email, string password, string firstName, string lastName, string? userType = "Guest");

    /// <summary>
    /// Autentica um usuário e gera token JWT
    /// </summary>
    /// <param name="email">Email do usuário</param>
    /// <param name="password">Senha do usuário</param>
    /// <param name="rememberMe">Manter conectado</param>
    /// <returns>Tupla com sucesso, token JWT e mensagem</returns>
    Task<(bool Success, string? Token, string Message)> AuthenticateAsync(string email, string password, bool rememberMe = false);

    /// <summary>
    /// Realiza logout do usuário
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <returns>Tupla com sucesso e mensagem</returns>
    Task<(bool Success, string Message)> LogoutAsync(string userId);

    /// <summary>
    /// Obtém informações do usuário autenticado
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <returns>Informações do usuário ou null se não encontrado</returns>
    Task<UserInfoDTO?> GetUserInfoAsync(string userId);

    /// <summary>
    /// Adiciona usuário a uma role específica
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <param name="role">Nome da role</param>
    /// <returns>Tupla com sucesso e mensagem</returns>
    Task<(bool Success, string Message)> AddUserToRoleAsync(string userId, string role);

    /// <summary>
    /// Obtém todas as roles de um usuário
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <returns>Lista de roles do usuário</returns>
    Task<IEnumerable<string>> GetUserRolesAsync(string userId);

    /// <summary>
    /// Verifica se usuário existe pelo email
    /// </summary>
    /// <param name="email">Email do usuário</param>
    /// <returns>True se usuário existe</returns>
    Task<bool> UserExistsAsync(string email);

    /// <summary>
    /// Obtém usuário pelo email
    /// </summary>
    /// <param name="email">Email do usuário</param>
    /// <returns>Informações do usuário ou null</returns>
    Task<UserInfoDTO?> GetUserByEmailAsync(string email);
}