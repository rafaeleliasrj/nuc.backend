namespace NautiHub.Domain.Services.DomainService;

public interface IAuthService
{
    public Guid GetUserId();
    public string? GetUserEmail();
    public string? GetUserName();
    public bool HasValidateUser();
}
