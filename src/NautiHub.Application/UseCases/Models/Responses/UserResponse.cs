using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Response com dados do usuário para API.
/// </summary>
public class UserResponse
{
    /// <summary>
    /// Identificador único do usuário.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome completo do usuário.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuário.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Nome de usuário para login.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Telefone do usuário (opcional).
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Indica se o email foi confirmado.
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// Data de nascimento do usuário.
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Tipo do usuário (Host, Guest, Admin).
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// Data do último login.
    /// </summary>
    public DateTime? LastLogin { get; set; }

    /// <summary>
    /// ID do perfil de host, se aplicável.
    /// </summary>
    public Guid? HostProfileId { get; set; }

    /// <summary>
    /// ID do perfil de guest, se aplicável.
    /// </summary>
    public Guid? GuestProfileId { get; set; }

    /// <summary>
    /// Data de criação do usuário.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data da última atualização.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}