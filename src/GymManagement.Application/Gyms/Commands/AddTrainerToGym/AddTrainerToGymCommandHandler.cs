using CSharpFunctionalExtensions;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Core.ErrorHandling;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.AddTrainerToGym;

public class AddTrainerToGymCommandHandler(
    IGymsRepository gymsRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddTrainerToGymCommand, UnitResult<OperationError>>
{
    private readonly IGymsRepository _gymsRepository = gymsRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<UnitResult<OperationError>> Handle(AddTrainerToGymCommand request, CancellationToken cancellationToken)
    {
        var gym = await _gymsRepository.GetByIdAsync(request.GymId, cancellationToken);
        
        if (gym is null)
        {
            return UnitResult.Failure(OperationError.NotFound("Gym not found"));
        }
        
        var addTrainerResult = gym.AddTrainer(request.TrainerId);
        
        if (!addTrainerResult.IsSuccess)
        {
            return addTrainerResult;
        }

        _gymsRepository.Update(gym);
        await _unitOfWork.CommitChangesAsync(cancellationToken);

        return UnitResult.Success<OperationError>();
    }
}