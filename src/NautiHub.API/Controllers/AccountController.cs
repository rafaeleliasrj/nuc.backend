using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Core.Interfaces;
using NautiHub.Core.Resources;

namespace NautiHub.API.Controllers;

/// <summary>
/// Controller para gerenciamento de contas de usuários
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IIdentityUserService _identityService;
    private readonly ILogger<AccountController> _logger;
    private readonly MessagesService _messagesService;

    /// <summary>
    /// Construtor do AccountController
    /// </summary>
    /// <param name="identityService">Serviço de identidade</param>
    /// <param name="logger">Logger para registro de eventos</param>
    /// <param name="messagesService">Serviço de mensagens localizadas</param>
    public AccountController(
        IIdentityUserService identityService,
        ILogger<AccountController> logger,
        MessagesService messagesService)
    {
        _identityService = identityService;
        _logger = logger;
        _messagesService = messagesService;
    }

    /// <summary>
    /// Registra um novo usuário no sistema
    /// </summary>
    /// <param name="input">Dados de registro do usuário</param>
    /// <returns>Resultado do registro</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest input)
    {
        try
        {
            var result = await _identityService.RegisterUserAsync(
                input.Email, 
                input.Password, 
                input.FirstName, 
                input.LastName);

            if (result.Success)
            {
                _logger.LogInformation("Usuário criado: {Email}", input.Email);
                return Ok(new { message = result.Message, userId = result.UserId });
            }

            return BadRequest(new { message = result.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _messagesService.Error_Registering_User);
            return StatusCode(500, _messagesService.Error_Internal_Server);
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest input)
    {
        try
        {
            var result = await _identityService.AuthenticateAsync(
                input.Email,
                input.Password,
                input.RememberMe);

            if (result.Success)
            {
                _logger.LogInformation("Usuário logado: {Email}", input.Email);
                return Ok(new { message = result.Message, token = result.Token });
            }

            if (result.Message.Contains("bloqueada") || result.Message.Contains("locked"))
            {
                _logger.LogWarning("Conta bloqueada: {Email}", input.Email);
                return BadRequest(result.Message);
            }

            return Unauthorized(result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _messagesService.Error_Login);
            return StatusCode(500, _messagesService.Error_Internal_Server);
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        var userId = User.FindFirst("UserId")?.Value;
if (string.IsNullOrEmpty(userId))
                return BadRequest(_messagesService.Auth_User_Not_Identified);

        var result = await _identityService.LogoutAsync(userId);
        if (result.Success)
        {
            _logger.LogInformation("Usuário deslogado: {UserId}", userId);
            return Ok(new { message = result.Message });
        }

            return BadRequest(new { message = result.Message });
    }

    [HttpGet("user-info")]
    [Authorize]
    public async Task<IActionResult> GetUserInfoAsync()
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userId))
            return NotFound();

        var userInfo = await _identityService.GetUserInfoAsync(userId);
        if (userInfo == null)
            return NotFound();

        return Ok(new
        {
            userInfo.Id,
            userInfo.Email,
            userInfo.FirstName,
            userInfo.LastName,
            userInfo.UserName,
            userInfo.Roles,
            userInfo.CreatedAt,
            userInfo.LastLogin
        });
    }
}