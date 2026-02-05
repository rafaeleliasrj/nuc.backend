namespace NautiHub.Core.Program;

public class InstanceIdentifier
{
    private readonly Guid _id;

    public InstanceIdentifier() => _id = Guid.NewGuid();

    public Guid Id => _id;

}
