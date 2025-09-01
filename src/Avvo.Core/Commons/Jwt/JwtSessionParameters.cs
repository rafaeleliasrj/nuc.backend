namespace Avvo.Core.Commons.Jwt;

public class JwtSessionParameters
{
    public Guid UserId { get; init; }
    public string Username { get; init; }
    public IReadOnlyDictionary<string, string> CustomClaims { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime ExpirationDate { get; init; }

    public JwtSessionParameters()
    {
        Username = string.Empty;
        CustomClaims = new Dictionary<string, string>().AsReadOnly();
        CreatedDate = DateTime.UtcNow;
        ExpirationDate = CreatedDate.AddMinutes(15);
    }

    public JwtSessionParameters(Guid userId, string username, IReadOnlyDictionary<string, string>? customClaims = null, TimeSpan? tokenLifetime = null)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("O ID do usuário não pode ser vazio.", nameof(userId));
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("O nome de usuário não pode ser nulo ou vazio.", nameof(username));

        UserId = userId;
        Username = username;
        CustomClaims = customClaims ?? new Dictionary<string, string>().AsReadOnly();
        CreatedDate = DateTime.UtcNow;
        ExpirationDate = CreatedDate.Add(tokenLifetime ?? TimeSpan.FromMinutes(15));
    }

    public JwtSessionParameters(Guid userId, string username, IReadOnlyDictionary<string, string>? customClaims, DateTime createdDate, DateTime expirationDate)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("O ID do usuário não pode ser vazio.", nameof(userId));
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("O nome de usuário não pode ser nulo ou vazio.", nameof(username));
        if (expirationDate <= createdDate)
            throw new ArgumentException("A data de expiração deve ser posterior à data de criação.", nameof(expirationDate));

        UserId = userId;
        Username = username;
        CustomClaims = customClaims ?? new Dictionary<string, string>().AsReadOnly();
        CreatedDate = createdDate;
        ExpirationDate = expirationDate;
    }
}
