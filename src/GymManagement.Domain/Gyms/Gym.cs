namespace GymManagement.Domain.Gyms;

public class Gym
{
    private readonly int _maxRooms;
    
    public Guid Id { get; }

    public string Name { get; init; } = null!;

    public Guid SubscriptionId { get; init; }
    
    private readonly List<Guid> _roomIds = new();
    
    private readonly List<Guid> _trainerIds = new();

    public Gym(string name, int maxRooms, Guid subscriptionId, Guid? id = null)
    {
        Name = name;
        _maxRooms = maxRooms;
        SubscriptionId = subscriptionId;
        Id = id ?? Guid.NewGuid();
    }
    
    private Gym() {}
}