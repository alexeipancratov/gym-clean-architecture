using CSharpFunctionalExtensions;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Core.ErrorHandling;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.CreateGym;

public class CreateGymCommandHandler(
    IUnitOfWork unitOfWork,
    IGymsRepository gymsRepository,
    ISubscriptionsRepository subscriptionsRepository)
    : IRequestHandler<CreateGymCommand, Result<Gym, OperationError>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IGymsRepository _gymsRepository = gymsRepository;
    private readonly ISubscriptionsRepository _subscriptionsRepository = subscriptionsRepository;

    public async Task<Result<Gym, OperationError>> Handle(CreateGymCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(request.SubscriptionId,
            cancellationToken);
        
        if (subscription is null)
        {
            return Result.Failure<Gym, OperationError>(OperationError.NotFound("Subscription not found"));
        }
        
        // TODO: Add user authorization check here
        
        var gym = new Gym(
            name: request.Name,
            maxRooms: subscription.GetMaxGyms(),
            subscriptionId: subscription.Id);

        var addGymResult = subscription.AddGym(gym);

        if (!addGymResult.IsSuccess)
        {
            // No need to convert domain errors here in this specific case
            // since we don't have any application-layer specific requirements for this validation.
            return Result.Failure<Gym, OperationError>(addGymResult.Error);
        }
        
        await _subscriptionsRepository.UpdateAsync(subscription, cancellationToken);
        await _gymsRepository.AddAsync(gym, cancellationToken);
        await _unitOfWork.CommitChangesAsync(cancellationToken);

        return Result.Success<Gym, OperationError>(gym);
    }
}