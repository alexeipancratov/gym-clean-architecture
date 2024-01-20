using CSharpFunctionalExtensions;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Core.ErrorHandling;
using GymManagement.Domain.Rooms;
using MediatR;

namespace GymManagement.Application.Rooms.Commands.CreateRoom;

public class CreateRoomCommandHandler(
    ISubscriptionsRepository subscriptionsRepository,
    IGymsRepository gymsRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateRoomCommand, Result<Room, OperationError>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository = subscriptionsRepository;
    private readonly IGymsRepository _gymsRepository = gymsRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<Result<Room, OperationError>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var gym = await _gymsRepository.GetByIdAsync(request.GymId, cancellationToken);
        if (gym is null)
        {
            return Result.Failure<Room, OperationError>(OperationError.NotFound("Gym not found."));
        }

        var subscription = await _subscriptionsRepository.GetByIdAsync(gym.SubscriptionId, cancellationToken);
        if (subscription is null)
        {
            return Result.Failure<Room, OperationError>(OperationError.NotFound("Subscription not found."));
        }

        var room = new Room(
            name: request.RoomName,
            gymId: gym.Id,
            maxDailySessions: subscription.GetMaxDailySessions());
        
        var addGymResult = gym.AddRoom(room);
        
        if (!addGymResult.IsSuccess)
        {
            return Result.Failure<Room, OperationError>(addGymResult.Error);
        }
        
        // Note: the room itself isn't stored in our database, but rather
        // in the "SessionManagement" system that is not in the scope of this course.
        _gymsRepository.Update(gym);
        await _unitOfWork.CommitChangesAsync(cancellationToken);

        return room;
    }
}