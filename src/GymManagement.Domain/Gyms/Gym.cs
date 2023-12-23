using Ardalis.Result;
using GymManagement.Domain.Rooms;
using Throw;

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

    public Result AddRoom(Room room)
    {
        _roomIds.Throw().IfContains(room.Id);
        
        if (_roomIds.Count >= _maxRooms)
        {
            return Result.Invalid(GymErrors.CannotHaveMoreRoomsThanSubscriptionAllows);
        }
        
        _roomIds.Add(room.Id);
        
        return Result.Success();
    }
    
    public Result AddTrainer(Guid trainerId)
    {
        if (_trainerIds.Contains(trainerId))
        {
            return Result.Conflict("Trainer already exists.");
        }
        
        _trainerIds.Add(trainerId);
        
        return Result.Success();
    }
}