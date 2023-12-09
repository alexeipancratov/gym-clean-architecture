using Ardalis.Result;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Commands;

public class CreateGymCommandHandler : IRequestHandler<CreateGymCommand, Result<Gym>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGymsRepository _gymsRepository;
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public CreateGymCommandHandler(
        IUnitOfWork unitOfWork,
        IGymsRepository gymsRepository,
        ISubscriptionsRepository subscriptionsRepository)
    {
        _unitOfWork = unitOfWork;
        _gymsRepository = gymsRepository;
        _subscriptionsRepository = subscriptionsRepository;
    }
    
    public async Task<Result<Gym>> Handle(CreateGymCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(request.SubscriptionId,
            cancellationToken);
        
        if (subscription is null)
        {
            return Result<Gym>.NotFound("Subscription not found");
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
            return addGymResult;
        }
        
        await _subscriptionsRepository.UpdateAsync(subscription, cancellationToken);
        await _gymsRepository.AddAsync(gym, cancellationToken);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        
        return Result<Gym>.Success(gym);
    }
}