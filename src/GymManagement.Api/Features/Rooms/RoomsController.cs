using CSharpFunctionalExtensions;
using GymManagement.Api.Shared;
using GymManagement.Application.Rooms.Commands.CreateRoom;
using GymManagement.Contracts.Rooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Features.Rooms;

[Route("gyms/{gymId:guid}/rooms")]
public class RoomsController(ISender sender) : ApiController
{
    private readonly ISender _sender = sender;
    
    [HttpPost]
    public async Task<IActionResult> CreateRoom(CreateRoomRequest request, Guid gymId)
    {
        var result = await _sender.Send(new CreateRoomCommand(gymId, request.Name));

        return result
            .Match(
                room => CreatedAtAction(
                    nameof(CreateRoom),
                    new { gymId, RoomId = room.Id},
                    new RoomResponse(room.Id, room.Name)),
                Problem);
    }
}