using CSharpFunctionalExtensions;
using GymManagement.Api.Shared;
using GymManagement.Application.Gyms.Commands.AddTrainerToGym;
using GymManagement.Application.Gyms.Commands.CreateGym;
using GymManagement.Contracts.Gyms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Features.Gyms;

[ApiController]
[Route("subscriptions/{subscriptionId:guid}/gyms")]
public class GymsController(ISender sender) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateGym(CreateGymRequest request, Guid subscriptionId)
    {
        var result = await sender.Send(new CreateGymCommand(request.Name, subscriptionId));

        return result.Match(
            gym => CreatedAtAction(
                nameof(CreateGym),
                new { subscriptionId, GymId = gym.Id},
                new GymResponse(gym.Id, gym.Name)),
            Problem);
    }
    
    [HttpPost("{gymId:guid}/trainers")]
    public async Task<IActionResult> AddTrainerToGym(Guid subscriptionId, Guid gymId, AddTrainerToGymRequest request)
    {
        var command = new AddTrainerToGymCommand(subscriptionId, gymId, request.TrainerId);
        var result = await sender.Send(command);
        
        return result.Match(
            Ok,
            Problem);
    }
}