using Ardalis.Result;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, Result<Subscription>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    // private readonly IUnitOfWork _unitOfWork;

    public CreateSubscriptionCommandHandler(
        ISubscriptionsRepository subscriptionsRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
        // _unitOfWork = unitOfWork;
    }

    public async Task<Result<Subscription>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = new Subscription
        {
            Id = Guid.NewGuid(),
            SubscriptionType = request.SubscriptionType
        };

        await _subscriptionsRepository.AddSubscription(subscription);

        // await _unitOfWork.CommitChangesAsync(cancellationToken);
        
        return subscription;
    }
}