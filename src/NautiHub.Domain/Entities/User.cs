using NautiHub.Core.DomainObjects;
using NautiHub.Core.Utils;
using NautiHub.Domain.Enums;
using NautiHub.Domain.Exceptions;

namespace NautiHub.Domain.Entities;

public class User : Entity, IAggregateRoot
{
    // Propriedades básicas
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string UserName { get; private set; }
    public string? PhoneNumber { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public UserType UserType { get; private set; }

    // Propriedades adicionais para compatibilidade com Identity
    public DateTime? LastLogin { get; private set; }
    public Guid? DomainUserId { get; private set; }
    public bool LockoutEnabled { get; private set; } = true;
    public DateTime? LockoutEnd { get; private set; }

    // Construtor privado para EF Core
    private User() { }

    public User(
        string email,
        string fullName,
        DateTime dateOfBirth,
        string userName = null,
        string phoneNumber = null)
    {
        Id = SequentialGuidGenerator.NewSequentialGuid();
        Email = email;
        UserName = userName ?? email;
        FullName = fullName;
        DateOfBirth = dateOfBirth;
        UserType = UserType.Guest;
        CreatedAt = DateTime.UtcNow;
        EmailConfirmed = false;
        PhoneNumber = phoneNumber;
    }

    // Métodos de domínio
    public void UpdateProfile(string fullName, DateTime dateOfBirth, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw UserDomainException.FullNameRequired();

        FullName = fullName;
        DateOfBirth = dateOfBirth;
        PhoneNumber = phoneNumber;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeUserType(UserType userType)
    {
        UserType = userType;
        UpdatedAt = DateTime.UtcNow;
    }

    // Métodos adicionais para gerenciamento de Identity
    public void UpdateLastLogin()
    {
        LastLogin = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetDomainUserId(Guid domainUserId)
    {
        DomainUserId = domainUserId;
        UpdatedAt = DateTime.UtcNow;
    }

    // Métodos para permitir conversão (usados pelo adapter)
    protected void SetId(Guid id) => Id = id;
    protected void SetEmailConfirmed(bool confirmed) => EmailConfirmed = confirmed;
    protected void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;
    protected void SetUpdatedAt(DateTime? updatedAt) => UpdatedAt = updatedAt;
    protected void SetUserType(UserType userType) => UserType = userType;
    protected void SetLastLogin(DateTime? lastLogin) => LastLogin = lastLogin;
    protected void SetLockoutEnabled(bool enabled) => LockoutEnabled = enabled;
    protected void SetLockoutEnd(DateTime? lockoutEnd) => LockoutEnd = lockoutEnd;

    
    public int CalculateAge()
    {
        var today = DateTime.Today;
        var age = today.Year - DateOfBirth.Year;

        if (DateOfBirth.Date > today.AddYears(-age))
            age--;

        return age;
    }
}
