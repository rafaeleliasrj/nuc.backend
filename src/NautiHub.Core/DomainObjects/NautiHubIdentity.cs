namespace NautiHub.Core.DomainObjects;

public class NautiHubIdentity : INautiHubIdentity
{
    public Guid? RequestId { get; private set; }
    public Guid? UserId { get; private set; }
    public string UserIp { get; private set; }

    public string? Email { get; private set; }
    public string? Name { get; private set; }

    public void SetRequestId(Guid requestId)
    {
        if (IsValidGuid(requestId))
            RequestId = requestId;
    }

    public void SetUserId(Guid usuarioId)
    {
        if (IsValidGuid(usuarioId))
            UserId = usuarioId;
    }

    public void SetUserIp(string userIp) => UserIp = userIp;

    public void SetEmail(string email) => Email = email;

    public void SetName(string nome) => Name = nome;

    private static bool IsValidGuid(Guid valor) => valor != Guid.Empty;
}
