using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using GymManagement.Application.Gyms.Commands;
using GymManagement.Contracts.Gyms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Features.Gyms;

[ApiController]
[Route("subscriptions/{subscriptionId:guid}/gyms")]
public class GymsController(ISender sender) : ControllerBase
{
    [HttpPost]
    [TranslateResultToActionResult]
    public async Task<Result<GymResponse>> CreateGym(CreateGymRequest request, Guid subscriptionId)
    {
        var result = await sender.Send(new CreateGymCommand(request.Name, subscriptionId));

        return result
            .Map(gym => new GymResponse(gym.Id, gym.Name));
    }
}