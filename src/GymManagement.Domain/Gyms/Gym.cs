using CSharpFunctionalExtensions;
using GymManagement.Core.ErrorHandling;
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

    public UnitResult<OperationError> AddRoom(Room room)
    {
        _roomIds.Throw().IfContains(room.Id);
        
        if (_roomIds.Count >= _maxRooms)
        {
            return UnitResult.Failure(GymErrors.CannotHaveMoreRoomsThanSubscriptionAllows);
        }
        
        _roomIds.Add(room.Id);

        return UnitResult.Success<OperationError>();
    }
    
    public UnitResult<OperationError> AddTrainer(Guid trainerId)
    {
        if (_trainerIds.Contains(trainerId))
        {
            return UnitResult.Failure(OperationError.Conflict("Trainer already exists"));
        }
        
        _trainerIds.Add(trainerId);

        return UnitResult.Success<OperationError>();
    }
}