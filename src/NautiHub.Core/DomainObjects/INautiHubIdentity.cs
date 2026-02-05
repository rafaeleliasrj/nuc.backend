namespace NautiHub.Core.DomainObjects;

public interface INautiHubIdentity
{
    public Guid? RequestId { get; }
    public Guid? UserId { get; }
    public string? UserIp { get; }
    public string? Email { get; }
    public string? Name { get; }

    public void SetRequestId(Guid requestId);
    public void SetUserId(Guid usuarioId);
    public void SetUserIp(string userIp);
    public void SetEmail(string email);
    public void SetName(string nome);
}
