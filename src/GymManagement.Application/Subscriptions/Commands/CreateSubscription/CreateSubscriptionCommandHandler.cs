using Ardalis.Result;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler(
    ISubscriptionsRepository subscriptionsRepository,
    IAdminsRepository adminsRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSubscriptionCommand, Result<Subscription>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository = subscriptionsRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAdminsRepository _adminsRepository = adminsRepository;

    public async Task<Result<Subscription>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var admin = await _adminsRepository.GetByIdAsync(request.AdminId);

        if (admin is null)
        {
            return Result.NotFound("Admin not found");
        }
        
        if (admin.SubscriptionId is not null)
        {
            return Result.Conflict("Admin already has a subscription");
        }
        
        var subscription = new Subscription(
            subscriptionType: request.SubscriptionType,
            adminId: request.AdminId);

        await _subscriptionsRepository.AddSubscription(subscription, cancellationToken);

        await _unitOfWork.CommitChangesAsync(cancellationToken);
        
        return subscription;
    }
}