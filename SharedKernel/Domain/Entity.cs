namespace SharedKernel.Domain;

public abstract class Entity<TId> where TId : notnull
{
    public TId Id { get; protected set; }

    // EF Core
    protected Entity() { }

    protected Entity(TId id)
    {
        Id = id;
    }
}
