namespace NautiHub.Core.DomainObjects;

public abstract class EntityUserControlAccess : Entity, IEntityUserControlAccess
{
    public Guid UserId { get; set; }
}
