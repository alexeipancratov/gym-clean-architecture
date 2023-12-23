using Ardalis.Result;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.AddTrainerToGym;

public class AddTrainerToGymCommandHandler(
    IGymsRepository gymsRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddTrainerToGymCommand, Result>
{
    private readonly IGymsRepository _gymsRepository = gymsRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<Result> Handle(AddTrainerToGymCommand request, CancellationToken cancellationToken)
    {
        Gym? gym = await _gymsRepository.GetByIdAsync(request.GymId, cancellationToken);
        
        if (gym is null)
        {
            return Result.NotFound("Gym not found");
        }
        
        Result addTrainerResult = gym.AddTrainer(request.TrainerId);
        
        if (!addTrainerResult.IsSuccess)
        {
            return addTrainerResult;
        }

        _gymsRepository.Update(gym);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}