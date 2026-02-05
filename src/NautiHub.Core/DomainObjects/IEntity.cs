namespace NautiHub.Core.DomainObjects;

public interface IEntity
{
    public Guid UserId { get; set; }
    public Guid Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
