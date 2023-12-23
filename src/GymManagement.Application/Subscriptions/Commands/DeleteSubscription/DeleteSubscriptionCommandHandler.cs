using Ardalis.Result;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.DeleteSubscription;

public class DeleteSubscriptionCommandHandler(
    ISubscriptionsRepository subscriptionsRepository,
    IAdminsRepository adminsRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteSubscriptionCommand, Result>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository = subscriptionsRepository;
    private readonly IAdminsRepository _adminsRepository = adminsRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<Result> Handle(DeleteSubscriptionCommand request, CancellationToken cancellationToken)
    {
        Subscription? subscription = await _subscriptionsRepository.GetByIdAsync(request.SubscriptionId,
            cancellationToken);
        
        if (subscription == null)
        {
            return Result.NotFound("Subscription not found");
        }
        
        Admin? admin = await _adminsRepository.GetByIdAsync(subscription.AdminId);
        
        if (admin == null)
        {
            return Result.NotFound("Admin not found");
        }
        
        admin.DeleteSubscription(subscription.Id);
        
        _adminsRepository.Update(admin);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        
        // TODO: Return Deleted status.
        return Result.Success();
    }
}