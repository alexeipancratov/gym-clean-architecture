using CSharpFunctionalExtensions;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Core.ErrorHandling;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.DeleteSubscription;

public class DeleteSubscriptionCommandHandler(
    ISubscriptionsRepository subscriptionsRepository,
    IAdminsRepository adminsRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteSubscriptionCommand, UnitResult<OperationError>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository = subscriptionsRepository;
    private readonly IAdminsRepository _adminsRepository = adminsRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<UnitResult<OperationError>> Handle(DeleteSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(request.SubscriptionId,
            cancellationToken);
        
        if (subscription == null)
        {
            return UnitResult.Failure(OperationError.NotFound("Subscription not found"));
        }
        
        var admin = await _adminsRepository.GetByIdAsync(subscription.AdminId);
        
        if (admin == null)
        {
            return UnitResult.Failure(OperationError.NotFound("Admin not found"));
        }
        
        admin.DeleteSubscription(subscription.Id);
        
        _adminsRepository.Update(admin);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        
        // TODO: Return Deleted status.
        return UnitResult.Success<OperationError>();
    }
}