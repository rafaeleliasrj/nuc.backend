namespace NautiHub.Infrastructure.Entities;

public class IdempotentCommand
{
    public Guid Id { get; set; }
    public DateTime ProcessadoEm { get; set; } = DateTime.UtcNow;
}
