using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using GymManagement.Application.Gyms.Commands;
using GymManagement.Contracts.Gyms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Features.Gyms;

[ApiController]
[Route("subscriptions/{subscriptionId:guid}/gyms")]
public class GymsController : ControllerBase
{
    private readonly ISender _sender;

    public GymsController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpPost]
    public async Task<ActionResult<GymResponse>> CreateGym(CreateGymRequest request, Guid subscriptionId)
    {
        var result = await _sender.Send(new CreateGymCommand(request.Name, subscriptionId));

        return result
            .Map(gym => new GymResponse(gym.Id, gym.Name))
            .ToActionResult(this);
    }
}