using NautiHub.Core.Utils;

namespace NautiHub.Core.DomainObjects;

public abstract class Entity(Guid? id = null) : IEntity
{
    public virtual Guid UserId { get; set; }
    public virtual Guid Id { get; set; } = id == null ? SequentialGuidGenerator.NewSequentialGuid() : id.Value;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public void MarkAsDeleted() => IsDeleted = true;

    public override bool Equals(object? obj)
    {
        var compareTo = obj as Entity;

        if (ReferenceEquals(this, compareTo))
            return true;
        if (compareTo is null)
            return false;

        return Id.Equals(compareTo.Id);
    }

    public static bool operator ==(Entity? a, Entity? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity? a, Entity? b) => !(a == b);

    public override int GetHashCode() => (GetType().GetHashCode() * 907) + Id.GetHashCode();

    public override string ToString() => $"{GetType().Name} [Id={Id}]";
}
