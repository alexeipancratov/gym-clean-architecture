namespace GymManagement.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; init; }

    protected List<IDomainEvent> DomainEvents = [];

    public Entity(Guid id) => Id = id;
    
    // Required for EF Core
    protected Entity() { }
}