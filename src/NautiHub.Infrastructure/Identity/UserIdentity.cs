using Microsoft.AspNetCore.Identity;
using NautiHub.Core.Utils;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Enums;

namespace NautiHub.Infrastructure.Identity;

/// <summary>
/// Classe de adaptador que herda de IdentityUser para integração com ASP.NET Core Identity
/// Mapeia para a entidade User do domínio
/// </summary>
public class UserIdentity : IdentityUser<Guid>
{
    // Propriedades do domínio
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public UserType UserType { get; set; } = UserType.Guest;
    public DateTime? LastLogin { get; set; }
    public Guid? DomainUserId { get; set; }



    // Métodos de conversão
    public User ToDomainUser()
    {
        return UserConverter.ToDomainUser(this);
    }

    public static UserIdentity FromDomainUser(User user)
    {
        return UserConverter.FromDomainUser(user);
    }
}

/// <summary>
/// Helper estático para conversão entre UserIdentity e User
/// </summary>
public static class UserConverter
{
    public static User ToDomainUser(UserIdentity userIdentity)
    {
        if (userIdentity == null)
            throw new ArgumentNullException(nameof(userIdentity));

        var user = new User(userIdentity.Email ?? string.Empty, userIdentity.FullName ?? string.Empty, userIdentity.DateOfBirth, userIdentity.UserName ?? string.Empty, userIdentity.PhoneNumber ?? string.Empty);
        
        // Usando reflection para acessar métodos protegidos (temporário)
        typeof(User).GetMethod("SetId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(user, new object[] { userIdentity.Id });
        typeof(User).GetMethod("SetEmailConfirmed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(user, new object[] { userIdentity.EmailConfirmed });
        typeof(User).GetMethod("SetCreatedAt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(user, new object[] { userIdentity.CreatedAt });
        typeof(User).GetMethod("SetUpdatedAt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(user, new object[] { userIdentity.UpdatedAt });
        typeof(User).GetMethod("SetUserType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(user, new object[] { userIdentity.UserType });
        typeof(User).GetMethod("SetLastLogin", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(user, new object[] { userIdentity.LastLogin });
        typeof(User).GetMethod("SetDomainUserId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(user, new object[] { userIdentity.DomainUserId });
        typeof(User).GetMethod("SetLockoutEnabled", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(user, new object[] { userIdentity.LockoutEnabled });
        typeof(User).GetMethod("SetLockoutEnd", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(user, new object[] { userIdentity.LockoutEnd?.DateTime });

        return user;
    }

    public static UserIdentity FromDomainUser(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return new UserIdentity
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber,
            EmailConfirmed = user.EmailConfirmed,
            FullName = user.FullName,
            DateOfBirth = user.DateOfBirth,
            CreatedAt = user.CreatedAt ?? DateTime.Now,
            UpdatedAt = user.UpdatedAt,
            UserType = user.UserType,
            LastLogin = user.LastLogin,
            DomainUserId = user.DomainUserId,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd.HasValue ? new DateTimeOffset(user.LockoutEnd.Value) : null
        };
    }
}