using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NautiHub.Core.DTOs;
using NautiHub.Core.Interfaces;
using NautiHub.Core.Resources;
using NautiHub.Infrastructure.Identity;

namespace NautiHub.Infrastructure.Services.Identity;

/// <summary>
/// Implementação concreta do serviço de identidade usando ASP.NET Core Identity
/// </summary>
public class IdentityUserService : IIdentityUserService
{
     private readonly UserManager<UserIdentity> _userManager;
     private readonly SignInManager<UserIdentity> _signInManager;
     private readonly IAuthenticationTokenService _tokenService;
     private readonly ILogger<IdentityUserService> _logger;
     private readonly MessagesService _messagesService;

     public IdentityUserService(
         UserManager<UserIdentity> userManager,
         SignInManager<UserIdentity> signInManager,
         IAuthenticationTokenService tokenService,
         ILogger<IdentityUserService> logger,
         MessagesService messagesService)
     {
         _userManager = userManager;
         _signInManager = signInManager;
         _tokenService = tokenService;
         _logger = logger;
         _messagesService = messagesService;
     }

     public async Task<(bool Success, string Message, string? UserId)> RegisterUserAsync(string email, string password, string firstName, string lastName, string? userType = "Guest")
     {
         try
         {
             if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                 return (false, _messagesService.Auth_Email_Password_Required, null);

             var existingUser = await _userManager.FindByEmailAsync(email);
             if (existingUser != null)
                 return (false, _messagesService.Auth_Email_In_Use, null);

            var userIdentity = new UserIdentity
            {
                Email = email,
                UserName = email,
                FullName = $"{firstName} {lastName}",
                DateOfBirth = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(userIdentity, password);

            if (result.Succeeded)
            {
                _logger.LogInformation(string.Format(_messagesService.Auth_User_Created, email));
                
                // Adicionar role padrão
                await _userManager.AddToRoleAsync(userIdentity, userType!);
                
                 return (true, _messagesService.Auth_User_Created, userIdentity.Id.ToString());
             }

             var errors = string.Join("; ", result.Errors.Select(e => e.Description));
             return (false, $"{_messagesService.Error_Registering_User}: {errors}", null);
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "Erro ao registrar usuário: {Email}", email);
             return (false, _messagesService.Error_Internal_Server, null);
         }
    }

    public async Task<(bool Success, string? Token, string Message)> AuthenticateAsync(string email, string password, bool rememberMe = false)
    {
        try
        {
            var result = await _signInManager.PasswordSignInAsync(
                email, password, rememberMe, lockoutOnFailure: false);

             if (result.Succeeded)
             {
                 var userIdentity = await _userManager.FindByEmailAsync(email);
                 if (userIdentity == null)
                     return (false, null, _messagesService.Auth_User_Not_Found);

                var roles = await _userManager.GetRolesAsync(userIdentity);
                var token = _tokenService.GenerateJwtToken(
                    userIdentity.Id.ToString(), 
                    userIdentity.Email!, 
                    userIdentity.UserName!, 
                    roles);

                // Atualizar último login
                userIdentity.LastLogin = DateTime.UtcNow;
                await _userManager.UpdateAsync(userIdentity);

                 _logger.LogInformation(string.Format(_messagesService.Auth_User_Logged_In, email));
                 return (true, token, _messagesService.Auth_User_Logged_In);
             }

             if (result.IsLockedOut)
                 return (false, null, _messagesService.Auth_Account_Locked);

             if (result.IsNotAllowed)
                 return (false, null, _messagesService.Auth_Account_Not_Allowed);

             return (false, null, _messagesService.Auth_Invalid_Credentials);
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "Erro ao autenticar usuário: {Email}", email);
             return (false, null, _messagesService.Error_Internal_Server);
         }
    }

     public async Task<(bool Success, string Message)> LogoutAsync(string userId)
     {
         try
         {
             await _signInManager.SignOutAsync();
              _logger.LogInformation(string.Format(_messagesService.Auth_Logout_Success, userId));
             return (true, _messagesService.Auth_Logout_Success);
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "Erro ao fazer logout do usuário: {UserId}", userId);
             return (false, _messagesService.Auth_Logout_Error);
         }
     }

    public async Task<UserInfoDTO?> GetUserInfoAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            var nameParts = (user.FullName ?? "").Split(' ', 2);
            var firstName = nameParts.Length > 0 ? nameParts[0] : "";
            var lastName = nameParts.Length > 1 ? nameParts[1] : "";

            return new UserInfoDTO
            {
                Id = user.Id.ToString(),
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin,
                DomainUserId = user.DomainUserId,
                Roles = roles,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd?.DateTime
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter informações do usuário: {UserId}", userId);
            return null;
        }
    }

     public async Task<(bool Success, string Message)> AddUserToRoleAsync(string userId, string role)
     {
         try
         {
             var user = await _userManager.FindByIdAsync(userId);
             if (user == null)
                 return (false, _messagesService.Auth_User_Not_Found);

             var result = await _userManager.AddToRoleAsync(user, role);
             if (result.Succeeded)
                 return (true, _messagesService.Auth_Add_Role_Success);

             var errors = string.Join("; ", result.Errors.Select(e => e.Description));
             return (false, $"{_messagesService.Auth_Add_Role_Error}: {errors}");
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "Erro ao adicionar usuário à role: {UserId}, {Role}", userId, role);
             return (false, _messagesService.Error_Internal_Server_Generic);
         }
     }

    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Enumerable.Empty<string>();

            return await _userManager.GetRolesAsync(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter roles do usuário: {UserId}", userId);
            return Enumerable.Empty<string>();
        }
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar existência do usuário: {Email}", email);
            return false;
        }
    }

    public async Task<UserInfoDTO?> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            var nameParts = (user.FullName ?? "").Split(' ', 2);
            var firstName = nameParts.Length > 0 ? nameParts[0] : "";
            var lastName = nameParts.Length > 1 ? nameParts[1] : "";

            return new UserInfoDTO
            {
                Id = user.Id.ToString(),
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin,
                DomainUserId = user.DomainUserId,
                Roles = roles,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd?.DateTime
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter usuário pelo email: {Email}", email);
            return null;
        }
    }
}